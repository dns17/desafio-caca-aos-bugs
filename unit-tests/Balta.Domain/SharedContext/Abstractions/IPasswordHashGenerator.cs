namespace Balta.Domain.SharedContext.Abstractions;

public interface IPasswordHashGenerator
{
    string Hash(string password);
}