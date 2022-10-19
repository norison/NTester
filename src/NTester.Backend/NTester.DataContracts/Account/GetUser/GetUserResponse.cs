namespace NTester.DataContracts.Account.GetUser;

/// <summary>
/// Response with the information about the user.
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// User name of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Surname of the user.
    /// </summary>
    public string Surname { get; set; } = string.Empty;
}