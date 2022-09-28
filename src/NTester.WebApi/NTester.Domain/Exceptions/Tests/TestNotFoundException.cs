using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Tests;

/// <summary>
/// Exception for the cases when the test was not found.
/// </summary>
public class TestNotFoundException : NotFoundException
{
    private const string ErrorMessage = "Test was not found - Test ID: '{0}'.";

    /// <summary>
    /// Creates an instance of the <see cref="TestNotFoundException"/>.
    /// </summary>
    /// <param name="id">Id of the test</param>
    public TestNotFoundException(Guid id) : base(string.Format(ErrorMessage, id))
    {
    }

    /// <inheritdoc cref="RestException.Code"/>
    public override int Code => (int)TestsCode.TestNotFound;
}