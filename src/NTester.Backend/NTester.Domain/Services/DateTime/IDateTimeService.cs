namespace NTester.Domain.Services.DateTime;

/// <summary>
/// Provides the date and time.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Current date and time in UTC.
    /// </summary>
    System.DateTime UtcNow { get; }
}