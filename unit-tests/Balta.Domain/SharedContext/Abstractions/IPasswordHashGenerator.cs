namespace Balta.Domain.SharedContext.Abstractions;

public interface IPasswordHashGenerator
{
    string Hash(string password, int saltSize = 16, int keySize = 32, int iterations = 10000);
}