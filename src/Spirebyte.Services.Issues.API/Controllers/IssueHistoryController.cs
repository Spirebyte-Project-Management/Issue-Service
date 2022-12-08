using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Issues.API.Controllers;

[Authorize]
[Route("issues/{issueId}/history")]
public class IssuesHistoryController : ApiController
{
    private readonly IDispatcher _dispatcher;

    public IssuesHistoryController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.IssueHistoryRead)]
    [SwaggerOperation("Browse issue history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<HistoryDto>> BrowseAsync(string issueId)
    {
        return Ok(await _dispatcher.QueryAsync(new GetHistory(issueId)));
    }
}