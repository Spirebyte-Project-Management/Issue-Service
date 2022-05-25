using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Services.Issues.API.Controllers.Base;
using Spirebyte.Services.Issues.Application.Issues.DTO;
using Spirebyte.Services.Issues.Application.Issues.Queries;
using Spirebyte.Services.Issues.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Spirebyte.Services.Issues.API.Controllers;

[Authorize]
[Route("issues/{issueId}/history")]
public class IssuesHistoryController : BaseController
{
    private readonly IDispatcher _dispatcher;

    public IssuesHistoryController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Authorize(ApiScopes.Read)]
    [SwaggerOperation("Browse issue history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<HistoryDto>> BrowseAsync(string issueId)
    {
        return Ok(await _dispatcher.QueryAsync(new GetHistory(issueId)));
    }
}