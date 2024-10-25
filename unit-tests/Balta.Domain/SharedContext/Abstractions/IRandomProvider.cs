namespace Balta.Domain.SharedContext.Abstractions;

public interface IRandomProvider
{
    int Next(int minValue, int maxValue);
}