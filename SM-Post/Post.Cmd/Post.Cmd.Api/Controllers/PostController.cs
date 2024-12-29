using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.Protocols;
using Post.Common.Protocols;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/posts")]
public class PostController(ICommandDispatcher commandDispatcher, ILogger<PostController> logger) : ControllerBase
{
    private readonly ILogger<PostController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] AddPostCommand command)
    {
        try
        {
            command.Id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status201Created, new NewPostResponse {Id = command.Id, Message = "New post creation request completed successfully" });
        }
        catch (InvalidOperationException exception)
        {
            _logger.Log(LogLevel.Warning, exception, "Client made a bad request!");
            return BadRequest(new NewPostResponse { Message = exception.Message });
        }
        catch (Exception exception)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request create a new post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse { Id = command.Id, Message = SAFE_ERROR_MESSAGE });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessagePost(Guid id, [FromBody] UpdateMessageCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Message post update successfully" });
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
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to update a  messge post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }


    [HttpPut("{id}/like")]
    public async Task<IActionResult> PostLikeAsync(Guid id)
    {
        try
        {
            var command = new LikePostCommand(id);
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Post liked successfully" });
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
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to like a post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }

    [HttpDelete("{PostId:guid}")]
    public async Task<IActionResult> RemovePostAsync([FromRoute] Guid PostId,[FromQuery] RemovePostCommand command)
    {
        try
        {
            command.Id = PostId;
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status200OK, new ResponseBase { Message = "Post removed successfully" });
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
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing the request to remove a post";
            _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = SAFE_ERROR_MESSAGE });
        }
    }
}
