namespace NTester.DataContracts;

/// <summary>
/// Response if any error occurred.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Message of the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional errors.
    /// </summary>
    public IEnumerable<ErrorResponse> Errors { get; set; } = new List<ErrorResponse>();
}