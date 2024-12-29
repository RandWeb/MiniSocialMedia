using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.Protocols;
using Post.Common.Protocols;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/posts/{PostId:guid}/comments")]
public sealed class CommentController(ILogger<CommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<CommentController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPost()]
    public async Task<IActionResult> CommentAddAsync([FromRoute] Guid PostId, [FromBody] AddCommentCommand command)
    {
        try
        {
            command.Id = PostId;
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Comment added to post successfully" });
        }
        catch (InvalidOperationException exception)
        {
            _logger.Log(LogLevel.Warning, exception, "Client made a bad request!");
            return BadRequest(new ResponseBase { Message = exception.Message });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrive aggregate. client passed an incorrect post id targetting the aggregate!");
            return BadRequest(new ResponseBase { Message = ex.Message });
        }
        catch (Exception exception)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to add a comment to a post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }

    [HttpPut("{CommentId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid PostId, Guid CommentId, UpdateCommentCommand command)
    {
        try
        {
            command.Id = PostId;
            command.CommentId = CommentId;
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Comment updated to post successfully" });
        }
        catch (InvalidOperationException exception)
        {
            _logger.Log(LogLevel.Warning, exception, "Client made a bad request!");
            return BadRequest(new ResponseBase { Message = exception.Message });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrive aggregate. client passed an incorrect post id targetting the aggregate!");
            return BadRequest(new ResponseBase { Message = ex.Message });
        }
        catch (Exception exception)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to update a comment to a post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }

    [HttpDelete("{CommentId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid PostId, Guid CommentId, [FromQuery] string userName)
    {
        try
        {
            var command = new RemoveCommentCommand { CommentId = CommentId, Id = PostId, UserName = userName };
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Comment deleted from post successfully" });
        }
        catch (InvalidOperationException exception)
        {
            _logger.Log(LogLevel.Warning, exception, "Client made a bad request!");
            return BadRequest(new ResponseBase { Message = exception.Message });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrive aggregate. client passed an incorrect post id targetting the aggregate!");
            return BadRequest(new ResponseBase { Message = ex.Message });
        }
        catch (Exception exception)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to delete a comment from a post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }
}
