using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Spirebyte.Services.Issues.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Issues.API.Controllers;

[Authorize]
public class IssuesController : ApiController
{
    private readonly IDispatcher _dispatcher;
    private readonly IIssueRequestStorage _issueRequestStorage;

    public IssuesController(IDispatcher dispatcher, IIssueRequestStorage issueRequestStorage)
    {
        _dispatcher = dispatcher;
        _issueRequestStorage = issueRequestStorage;
    }

    [HttpGet]
    [Authorize(ApiScopes.IssuesRead)]
    [SwaggerOperation("Browse issues")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IssueDto>> BrowseAsync([FromQuery] GetIssues query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{issueId}")]
    [Authorize(ApiScopes.IssuesRead)]
    [SwaggerOperation("Get issue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto?>> GetAsync(string issueId)
    {
        return await _dispatcher.QueryAsync(new GetIssue(issueId));
    }

    [HttpPost]
    [Authorize(ApiScopes.IssuesWrite)]
    [SwaggerOperation("Create issue")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateIssue(CreateIssue command)
    {
        await _dispatcher.SendAsync(command);
        var issue = _issueRequestStorage.GetIssue(command.ReferenceId);
        return Created($"issues/{issue.Id}", issue);
    }

    [HttpPut("{issueId}")]
    [Authorize(ApiScopes.IssuesWrite)]
    [SwaggerOperation("Update issue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateIssue(string issueId, UpdateIssue command)
    {
        if (!command.Id.Equals(issueId)) return NotFound();

        await _dispatcher.SendAsync(command);
        return Ok();
    }

    [HttpDelete("{issueId}")]
    [Authorize(ApiScopes.IssuesDelete)]
    [SwaggerOperation("Delete issue")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteIssue(string issueId)
    {
        await _dispatcher.SendAsync(new DeleteIssue(issueId));
        return Ok();
    }
}