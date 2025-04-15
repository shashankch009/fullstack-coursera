using System;
using System.Collections.Generic;
using EventEase.Models;

namespace EventEase.Services;

public class AuthService
{
    private readonly Dictionary<string, User> _users = new();
    private string _currentToken;

    public bool Register(RegisterModel registerInfo)
    {
        Console.WriteLine($"Attempting to register user: {registerInfo.Email}");
        if (_users.ContainsKey(registerInfo.Email))
        {
            Console.WriteLine($"user already registered: {registerInfo.Email}");
            return false; // Email already registered
        }
        var user = new User
        {
            Id = GetNextUserId(),
            FullName = registerInfo.FullName,
            Email = registerInfo.Email,
            Password = registerInfo.Password // In a real application, hash the password
        };

       _users[user.Email] = user;

        Console.WriteLine($"User Registered: {user.FullName}, Email: {user.Email}");
        return true;
    }

    public string Login(LoginModel loginInfo, out User loggedInUser)
    {
        Console.WriteLine($"Attempting to log in user: {loginInfo.Email}");
        loggedInUser = null;
        if (_users.TryGetValue(loginInfo.Email, out var user) && user.Password == loginInfo.Password)
        {
            _currentToken = Guid.NewGuid().ToString(); // Generate token
            loggedInUser = user;
            Console.WriteLine($"User Logged In: {user.FullName}, Email: {user.Email}");
            return _currentToken;
        }
        Console.WriteLine($"Failed to log in user: {loginInfo.Email}");
        return null; // Invalid credentials
    }

    public bool IsAuthenticated(string token)
    {
        return token == _currentToken; // Check session token
    }

    public void Logout()
    {
        _currentToken = null;
    }

    private int GetNextUserId()
    {
        return _users.Count > 0 ? _users.Values.Max(u => u.Id) + 1 : 1;
    }
}
