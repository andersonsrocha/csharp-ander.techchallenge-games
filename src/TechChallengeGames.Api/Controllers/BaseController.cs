using System.Net;
using Microsoft.AspNetCore.Mvc;
using OperationResult;

namespace TechChallengeGames.Api.Controllers;

[ApiController]
public class BaseController(ILogger<BaseController> logger) : ControllerBase
{
    protected IActionResult Send(object? value)
        => value switch
        {
            null => Problem(statusCode: (int)HttpStatusCode.NotFound),
            _ => Ok(value)
        };

    protected async Task<IActionResult> Send(Task<Result<Guid>> task)
        => await task switch
        {
            (true, var result) => Created(string.Empty, result),
            (false, _, var exception) => TreatError(exception),
            _ => Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    
    protected async Task<IActionResult> Send<TResponse>(Task<Result<TResponse>> task)
        => await task switch
        {
            (true, var result) => Ok(result),
            (false, _, var exception) => TreatError(exception),
            _ => Problem(statusCode: (int)HttpStatusCode.InternalServerError)
        };
    
    protected async Task<IActionResult> Send(Task<Result> task)
        => await task switch
        {
            (true, _) => Ok(),
            (false, var exception) => TreatError(exception)
        };

    [NonAction]
    private ObjectResult TreatError(Exception? error)
    {
        switch (error)
        {
            case not null:
                logger.LogError("An unexpected fault happened, please contact yor Administrator with the error id: {ErrorMessage}", error.Message);
                return Problem(detail: error.Message, statusCode: (int)HttpStatusCode.BadRequest);
            default:
                logger.LogError("An unexpected fault happened, please contact yor Administrator with the error id.");
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}