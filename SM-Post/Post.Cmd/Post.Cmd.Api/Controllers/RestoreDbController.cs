using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.Protocols;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/restore")]
public sealed class RestoreDbController(
    ICommandDispatcher dispatcher,
    ILogger<RestoreDbCommand> logger
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RestoreReadDbAsync()
    {
        try
        {
            var command = new RestoreDbCommand();
            await dispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status201Created, new ResponseBase { Message = "Read restore request completed successfully" });
        }
        catch (InvalidOperationException exception)
        {
            logger.Log(LogLevel.Warning, exception, "Client made a bad request!");
            return BadRequest(new ResponseBase { Message = exception.Message });
        }
        catch (Exception exception)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to restore read database";
            logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }
}
