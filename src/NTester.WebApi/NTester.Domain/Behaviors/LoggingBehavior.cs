using MediatR;
using Microsoft.Extensions.Logging;
using NTester.Domain.Services.DateTime;

namespace NTester.Domain.Behaviors;

/// <summary>
/// Provides logging for commands and queries.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// Creates an instance of the <see cref="LoggingBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="logger">Instance of the logger.</param>
    /// <param name="dateTimeService">Provides date and time.</param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IDateTimeService dateTimeService)
    {
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    /// <inheritdoc cref="IPipelineBehavior{TRequest,TResponse}.Handle"/>
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        var uniqueId = Guid.NewGuid().ToString();
        var startDateTime = _dateTimeService.UtcNow;

        try
        {
            LogRequest(uniqueId, requestName);

            var response = await next();

            LogResponse(uniqueId, requestName, startDateTime);

            return response;
        }
        catch (Exception exception)
        {
            LogResponse(exception, uniqueId, requestName, startDateTime);
            throw;
        }
    }

    private void LogRequest(string uniqueId, string requestName)
    {
        const string template = "Begin Request Id: '{uniqueId}', Request name: '{requestName}'";
        _logger.LogInformation(template, uniqueId, requestName);
    }

    private void LogResponse(string uniqueId, string requestName, DateTime startDateTime)
    {
        var message = GetResponseMessage(uniqueId, requestName, startDateTime);
        _logger.LogInformation(message);
    }

    private void LogResponse(Exception exception, string uniqueId, string requestName, DateTime startDateTime)
    {
        var message = GetResponseMessage(uniqueId, requestName, startDateTime);
        _logger.LogInformation(exception, message);
    }

    private string GetResponseMessage(string uniqueId, string requestName, DateTime startDateTime)
    {
        var time = (_dateTimeService.UtcNow - startDateTime).TotalMilliseconds;
        return $"End Request Id: '{uniqueId}', Request name: '{requestName}', Total request time: '{time}'";
    }
}