using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeProcurement.Services.Contracts.Exceptions;

namespace OfficeProcurement.Infrastructure;

/// <summary>
/// Фильтр исключений для преобразования бизнес-исключений и системных ошибок в HTTP-ответы (ProblemDetails)
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> logger;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ExceptionFilter"/>
    /// </summary>
    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        logger.LogError(exception, "Необработанное исключение: {ExceptionType}", exception.GetType().Name);

        switch (exception)
        {
            case NotFoundException ex:
                SetProblemDetails(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
                break;

            case OfficeProcurementConflictException ex:
                SetProblemDetails(context, StatusCodes.Status409Conflict, "Conflict", ex.Message);
                break;

            case OfficeProcurementInvalidOperationException ex:
                SetProblemDetails(context, StatusCodes.Status406NotAcceptable, "Invalid Operation", ex.Message);
                break;

            case OfficeProcurementValidationException ex:
                var errors = ex.Errors
                    .GroupBy(e => e.Field)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Message).ToArray());

                var validationProblem = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Validation Failed",
                    Detail = ex.Message
                };

                SetDataToContext(new ObjectResult(validationProblem)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                }, context);
                break;

            case OfficeProcurementException ex:
                SetProblemDetails(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
                break;

            default:
                SetProblemDetails(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "Произошла внутренняя ошибка сервера");
                break;
        }
    }

    private static void SetProblemDetails(ExceptionContext context, int statusCode, string title, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        SetDataToContext(new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        }, context);
    }

    private static void SetDataToContext(ObjectResult data, ExceptionContext context)
    {
        context.ExceptionHandled = true;
        context.HttpContext.Response.StatusCode = data.StatusCode ?? StatusCodes.Status400BadRequest;
        context.Result = data;
    }
}
