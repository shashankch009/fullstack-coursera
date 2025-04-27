
namespace EncryptionDemo.Services;

public interface IEncryptionService 
{
    string Encrypt(string plainText);
    string Decrypt(string cipher);
}