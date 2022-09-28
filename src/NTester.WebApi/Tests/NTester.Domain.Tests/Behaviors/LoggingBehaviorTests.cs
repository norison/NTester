using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NTester.Domain.Behaviors;
using NUnit.Framework;

namespace NTester.Domain.Tests.Behaviors;

[TestFixture]
public class LoggingBehaviorTests
{
    private ILogger<LoggingBehavior<TestRequest, Unit>> _logger;
    private RequestHandlerDelegate<Unit> _nextDelegate;
    private LoggingBehavior<TestRequest, Unit> _loggingBehavior;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<LoggingBehavior<TestRequest, Unit>>>();
        _nextDelegate = Substitute.For<RequestHandlerDelegate<Unit>>();

        _loggingBehavior = new LoggingBehavior<TestRequest, Unit>(_logger);
    }

    [Test]
    public async Task Handle_NoErrors_ShouldLogRequestAndResponse()
    {
        // Act
        await _loggingBehavior.Handle(new TestRequest(), CancellationToken.None, _nextDelegate);

        // Assert
        _logger.ReceivedCalls().Count().Should().Be(2);
    }
    
    [Test]
    public async Task Handle_Error_ShouldLogRequestAndError()
    {
        // Arrange
        _nextDelegate.Invoke().ThrowsAsync<Exception>();
        
        // Act/Assert
        await _loggingBehavior
            .Invoking(x => x.Handle(new TestRequest(), CancellationToken.None, _nextDelegate))
            .Should()
            .ThrowAsync<Exception>();
        
        _logger.ReceivedCalls().Count().Should().Be(2);
    }

    public class TestRequest : IRequest<Unit>
    {
    }
}