using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Issues.API.Controllers.Base;
using Spirebyte.Services.Issues.Application.IssueComments.Commands;
using Spirebyte.Services.Issues.Application.IssueComments.DTO;
using Spirebyte.Services.Issues.Application.IssueComments.Queries;
using Spirebyte.Services.Issues.Application.IssueComments.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Issues.API.Controllers;

[Authorize]
[Route("issues/{issueId}/comments")]
public class IssuesCommentsController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IIssueCommentsRequestStorage _issueCommentsRequestStorage;

    public IssuesCommentsController(IDispatcher dispatcher, IIssueCommentsRequestStorage issueCommentsRequestStorage)
    {
        _dispatcher = dispatcher;
        _issueCommentsRequestStorage = issueCommentsRequestStorage;
    }

    [HttpGet]
    [SwaggerOperation("Browse issue comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CommentDto>> BrowseAsync(string issueId)
    {
        return Ok(await _dispatcher.QueryAsync(new GetComments(null, issueId)));
    }

    [HttpGet("{commentId}")]
    [SwaggerOperation("Get issue comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CommentDto>> GetAsync(string issueId, string commentId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetComment(commentId)));
    }

    [HttpPost]
    [SwaggerOperation("Create issue comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateComment(string issueId, CreateComment command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.IssueId, issueId));
        var comment = _issueCommentsRequestStorage.GetComment(command.ReferenceId);
        return Created($"issues/{issueId}/comments/{comment.Id}", comment);
    }

    [HttpPut("{commentId}")]
    [SwaggerOperation("Update issue comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateComment(string issueId, string commentId, UpdateComment command)
    {
        if (!command.Id.Equals(commentId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{commentId}")]
    [SwaggerOperation("Delete issue comment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteComment(string issueId, string commentId)
    {
        await _dispatcher.SendAsync(new DeleteComment(commentId));
        return Ok();
    }
}