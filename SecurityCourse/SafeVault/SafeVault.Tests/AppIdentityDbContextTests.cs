using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using MySqlConnector;
using NUnit.Framework;

namespace SafeVault.Tests;

[TestFixture]
public class AppIdentityDbContextTests
{
    private AppIdentityDbContext _dbContext;
    private Mock<IConfiguration> _mockConfiguration;

    [SetUp]
    public void SetUp()
    {
        // Mock the configuration to provide a connection string
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["ConnectionStrings:DefaultConnection"])
            .Returns("Server=localhost;Database=SafeVaultTestDb;User=root;Password=Test@123;");

        // Set up the DbContext options
        var options = new DbContextOptionsBuilder<AppIdentityDbContext>()
            .UseInMemoryDatabase("SafeVaultTestDb")
            .Options;

        _dbContext = new AppIdentityDbContext(options, _mockConfiguration.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public void VerifyUser_ShouldPreventSqlInjection()
    {
        // Arrange: Simulate a malicious SQL injection input
        string maliciousUsername = "'; DROP TABLE AspNetUsers; --";
        string password = "Test@123";

        // Act
        bool result = _dbContext.VerifyUser(maliciousUsername, password);

        // Assert: The method should not execute the malicious SQL
        Assert.That(result, Is.False, "VerifyUser should prevent SQL injection attacks.");
    }

    [Test]
    public void VerifyUser_ShouldPreventXSS()
    {
        // Arrange: Simulate a malicious XSS input
        string maliciousUsername = "<script>alert('XSS');</script>";
        string password = "Test@123";

        // Act
        bool result = _dbContext.VerifyUser(maliciousUsername, password);

        // Assert: The method should not process the malicious input
        Assert.That(result, Is.False, "VerifyUser should prevent XSS attacks.");
    }
}