namespace NTester.DataContracts.Auth.Refresh;

/// <summary>
/// Request for a new pair of tokens.
/// </summary>
public class RefreshRequest
{
    /// <summary>
    /// Token for accessing the resource.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}