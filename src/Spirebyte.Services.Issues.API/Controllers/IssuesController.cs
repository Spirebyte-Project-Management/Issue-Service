﻿using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Issues.API.Controllers.Base;
using Spirebyte.Services.Issues.Application.Issues.Commands;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Application.Issues.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Issues.API.Controllers;

[Authorize]
public class IssuesController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IIssueRequestStorage _issueRequestStorage;

    public IssuesController(IDispatcher dispatcher, IIssueRequestStorage issueRequestStorage)
    {
        _dispatcher = dispatcher;
        _issueRequestStorage = issueRequestStorage;
    }

    [HttpGet]
    [SwaggerOperation("Browse issues")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IssueDto>> BrowseAsync([FromQuery] GetIssues query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("{issueId}")]
    [SwaggerOperation("Get issue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IssueDto>> GetAsync(string issueId)
    {
        return OkOrNotFound(await _dispatcher.QueryAsync(new GetIssue(issueId)));
    }

    [HttpPost]
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
    [SwaggerOperation("Delete issue")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteIssue(string issueId)
    {
        await _dispatcher.SendAsync(new DeleteIssue(issueId));
        return Ok();
    }
}