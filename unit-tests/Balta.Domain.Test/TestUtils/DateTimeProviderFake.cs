using Balta.Domain.SharedContext.Abstractions;

namespace Balta.Domain.Test.TestUtils;

internal class DateTimeProviderFake(DateTime? fixedDateTime = null)
: IDateTimeProvider
{
    public DateTime UtcNow => fixedDateTime ?? DateTime.UtcNow;
}