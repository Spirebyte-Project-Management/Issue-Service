using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Commands;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Infrastructure;

namespace Spirebyte.Services.Issues.API
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UsePingEndpoint()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetIssues, IEnumerable<IssueDto>>("issues/{projectId:guid}")
                        .Post<CreateIssue>("issues",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"issues/{cmd.IssueId}"))
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
