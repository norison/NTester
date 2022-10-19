using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using NTester.Domain.Behaviors;
using NUnit.Framework;
using ValidationException = NTester.Domain.Exceptions.Base.ValidationException;

namespace NTester.Domain.Tests.Behaviors;

[TestFixture]
public class ValidationBehaviorTests
{
    private IValidator<TestRequest> _validator;
    private RequestHandlerDelegate<Unit> _nextDelegate;
    private ValidationBehavior<TestRequest, Unit> _validationBehavior;

    [SetUp]
    public void SetUp()
    {
        _validator = Substitute.For<IValidator<TestRequest>>();
        _nextDelegate = Substitute.For<RequestHandlerDelegate<Unit>>();

        _validationBehavior = new ValidationBehavior<TestRequest, Unit>(new[] { _validator });
    }

    [Test]
    public async Task Handle_NoErrors_ShouldCallNextDelegate()
    {
        // Arrange
        var request = new TestRequest();

        // ReSharper disable once MethodHasAsyncOverload
        _validator.Validate((IValidationContext)default!).ReturnsForAnyArgs(new ValidationResult());

        // Act
        await _validationBehavior.Handle(request, _nextDelegate, CancellationToken.None);

        // Assert
        _nextDelegate.ReceivedCalls().Count().Should().Be(1);
    }

    [Test]
    public async Task Handle_Error_ShouldThrowBadRequestException()
    {
        // Arrange
        const string errorMessage = "testErrorMessage";
        var request = new TestRequest();
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("test property", errorMessage)
        });

        // ReSharper disable once MethodHasAsyncOverload
        _validator.Validate((IValidationContext)default!).ReturnsForAnyArgs(validationResult);

        // Act/Assert
        await _validationBehavior
            .Invoking(x => x.Handle(request, _nextDelegate, CancellationToken.None))
            .Should()
            .ThrowAsync<ValidationException>()
            .WithMessage(errorMessage);

        _nextDelegate.ReceivedCalls().Count().Should().Be(0);
    }

    public class TestRequest : IRequest<Unit>
    {
    }
}