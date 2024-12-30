using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.Protocols;
using Post.Query.Api.Protocols;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/posts")]
public sealed class PostController(IQueryDispacher<PostEntity> queryDispacher, ILogger<PostController> logger) : ControllerBase
{
    private readonly IQueryDispacher<PostEntity> _queryDispacher = queryDispacher;
    private readonly ILogger<PostController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
        try
        {
            var posts = await _queryDispacher.SendAsync(new FindAllPostsQuery());

            return SuccessResponse(posts);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (AggregateException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing your request to retrive All Posts.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute ] FindPostByIdQuery query)
    {
        try
        {
            var posts = await _queryDispacher.SendAsync(query);
            var count = posts?.Count;
            if (count is null or 0) return NoContent();
            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully returned post."
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (AggregateException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing your request to retrive a post.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("by-author")]
    public async Task<IActionResult> GetPostsByAuthorAsync([FromQuery] FindPostsByAuthorQuery query)
    {
        try
        {
            var posts = await _queryDispacher.SendAsync(query);
            return SuccessResponse(posts);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (AggregateException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing your request to retrive posts by author.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("with-comments")]
    public async Task<IActionResult> GetPostsWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispacher.SendAsync(new FindPostsWithCommentsQuery());
            return SuccessResponse(posts);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (AggregateException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing your request to retrive posts with comments.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("by-numberOfLikes")]
    public async Task<IActionResult> GetPostsByNumberOfLikesAsync([FromQuery] FindPostsWithLikesQuery query)
    {
        try
        {
            var posts = await _queryDispacher.SendAsync(query);
            return SuccessResponse(posts);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (AggregateException ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "An error occurred while processing your request to retrive posts by number of likes.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    private IActionResult SuccessResponse(List<PostEntity>? posts)
    {
        var count = posts?.Count;
        if (count is null or 0) return NoContent();
        return Ok(new PostLookupResponse
        {
            Posts = posts,
            Message = $"Successfully returned {count} post {(count > 1 ? "s" : string.Empty)}"
        });
    }

    private IActionResult ErrorResponse(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase { Message = safeErrorMessage });
    }
}
