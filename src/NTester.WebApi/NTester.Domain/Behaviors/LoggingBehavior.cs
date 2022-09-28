using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace NTester.Domain.Behaviors;

/// <summary>
/// Provides logging for commands and queries.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const string BeginRequestTemplate = "Begin Request Id: '{uniqueId}', Request name: '{requestName}'";

    private const string EndRequestTemplate =
        "End Request Id: '{uniqueId}', Request name: '{requestName}', Total request time: '{time}'";

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Creates an instance of the <see cref="LoggingBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="logger">Instance of the logger.</param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc cref="IPipelineBehavior{TRequest,TResponse}.Handle"/>
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        var uniqueId = Guid.NewGuid().ToString();
        var stopWatch = new Stopwatch();

        try
        {
            _logger.LogInformation(BeginRequestTemplate, uniqueId, requestName);

            stopWatch.Start();
            var response = await next();
            stopWatch.Stop();

            _logger.LogInformation(EndRequestTemplate, uniqueId, requestName, stopWatch.Elapsed);

            return response;
        }
        catch (Exception exception)
        {
            stopWatch.Stop();
            _logger.LogError(exception, EndRequestTemplate, uniqueId, requestName, stopWatch.Elapsed);
            throw;
        }
    }
}