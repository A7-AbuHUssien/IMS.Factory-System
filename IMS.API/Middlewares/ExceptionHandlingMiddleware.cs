using System.Text.Json;
using AutoMapper;
using IMS.Application.Common.DTOs;
using IMS.Domain.Exceptions;
using Microsoft.Data.SqlClient;

namespace IMS.API.Middlewares;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (SqlException)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Database error occurred");
        }
        catch (ArgumentException)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "INVALID_DATA");
        }
        catch (AutoMapperMappingException)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Mapping error occurred");
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>(message)
        {
            Errors = new List<string> { message }
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}