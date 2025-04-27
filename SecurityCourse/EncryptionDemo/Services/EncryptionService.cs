
using System.Security.Cryptography;
using System.Text;

namespace EncryptionDemo.Services;

public class EncryptionService  : IEncryptionService
{
    private readonly byte[] key;
    private readonly byte[] iv; //initial vector 

    public EncryptionService(IConfiguration configuration)
    {
        key = Encoding.UTF8.GetBytes(configuration["Encryption:Key"]);
        iv = Encoding.UTF8.GetBytes(configuration["Encryption:IV"]);
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms  = new MemoryStream();
        using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream))
        {
            writer.Write(plainText);
            writer.Flush();
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipher)
    {
        var buffer = Convert.FromBase64String(cipher);
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms  = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        
        return reader.ReadToEnd();
    }
}