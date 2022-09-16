using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NTester.DataContracts;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Common;
using NTester.WebApi.Middlewares.CustomExceptionHandler;
using NUnit.Framework;

namespace NTester.WebApi.Tests.Middlewares;

[TestFixture]
public class CustomExceptionHandlerMiddlewareTests
{
    private RequestDelegate _requestDelegate;
    private CustomExceptionHandlerMiddleware _middleware;

    [SetUp]
    public void SetUp()
    {
        _requestDelegate = Substitute.For<RequestDelegate>();

        _middleware = new CustomExceptionHandlerMiddleware(_requestDelegate);
    }

    [Test]
    public async Task InvokeAsync_NoErrors_ShouldInvokeTheRequestDelegate()
    {
        // Arrange
        var context = CreateHttpContext();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _requestDelegate.Received().Invoke(context);
    }

    [Test, TestCaseSource(nameof(RestExceptions))]
    public async Task InvokeAsync_ThrowsRestExceptionBase_ShouldHandleException(RestExceptionBase restExceptionBase)
    {
        // Arrange
        var context = CreateHttpContext();
        _requestDelegate.Invoke(default!).ThrowsForAnyArgs(restExceptionBase);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _requestDelegate.Received().Invoke(context);
        
        context.Response.StatusCode.Should().Be((int)restExceptionBase.StatusCode);
        context.Response.ContentType.Should().Be("application/json; charset=utf-8");

        await AssertRestException(context, restExceptionBase);
    }

    [Test]
    public async Task InvokeAsync_ThrowsUnknownException_ShouldHandleException()
    {
        // Arrange
        const string message = "unknown test exception message";

        var exception = new Exception(message);
        var expectedException = new NonGeneralException(message);
        var context = CreateHttpContext();

        _requestDelegate.Invoke(default!).ThrowsForAnyArgs(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        await _requestDelegate.Received().Invoke(context);
        
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json; charset=utf-8");

        await AssertRestException(context, expectedException);
    }

    private static async Task AssertRestException(HttpContext context, RestExceptionBase restExceptionBase)
    {
        var errorResponse = await GetErrorResponse(context);
        errorResponse.Message.Should().Be(restExceptionBase.Message);
        errorResponse.Code.Should().Be(restExceptionBase.Code);
        errorResponse.Description.Should().Be(restExceptionBase.Description);
    }

    private static HttpContext CreateHttpContext()
    {
        return new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
    }

    private static async Task<ErrorResponse> GetErrorResponse(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var streamText = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<ErrorResponse>(streamText);
    }

    private static IEnumerable<RestExceptionBase> RestExceptions => new List<RestExceptionBase>
    {
        new ModelValidationException("validation message"),
        new NonGeneralException("non-general message")
    };
}