﻿using System.Net;
using NTester.DataContracts;
using NTester.Domain.Exceptions;

namespace NTester.WebApi.Middlewares.CustomExceptionHandler;

/// <summary>
/// Middleware to handle all exceptions in the application.
/// </summary>
public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Creates an instance of the custom exception handler.
    /// </summary>
    /// <param name="next">Next middleware action.</param>
    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the exception middleware.
    /// </summary>
    /// <param name="context">context of the request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        if (exception is RestException restException)
        {
            statusCode = restException.StatusCode;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Message = exception.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}