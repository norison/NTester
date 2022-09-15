using System.Diagnostics.CodeAnalysis;

namespace NTester.Domain.Services.DateTime;

/// <inheritdoc cref="IDateTimeService"/>
[ExcludeFromCodeCoverage]
public class DateTimeService : IDateTimeService
{
    /// <inheritdoc cref="IDateTimeService.UtcNow"/>
    public System.DateTime UtcNow => System.DateTime.UtcNow;
}