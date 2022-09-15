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
    /// Code of the error.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Description of the error code.
    /// </summary>
    public string CodeDescription { get; set; } = string.Empty;
}