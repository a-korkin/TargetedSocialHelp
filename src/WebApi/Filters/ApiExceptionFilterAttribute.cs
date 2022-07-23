using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(AlreadyExistsException), HandleAlreadyExistsException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request."
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            Title = "The specified resourse was not found",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void HandleAlreadyExistsException(ExceptionContext context)
    {
        var exception = (AlreadyExistsException)context.Exception;

        var details = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            Title = "Resourse with same unique property already exists",
            Detail = exception.Message
        };

        context.Result = new ConflictObjectResult(details);

        context.ExceptionHandled = true;
    }
}