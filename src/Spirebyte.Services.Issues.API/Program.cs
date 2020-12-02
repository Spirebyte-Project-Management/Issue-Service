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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Open.Serialization.Json;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Application.Commands;
using Spirebyte.Services.Issues.Application.DTO;
using Spirebyte.Services.Issues.Application.Queries;
using Spirebyte.Services.Issues.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.API
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IJsonSerializer GetJsonSerializer()
        {
            var factory = new Open.Serialization.Json.Newtonsoft.JsonSerializerFactory(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return factory.GetSerializer();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi(jsonSerializer: GetJsonSerializer())
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UsePingEndpoint()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetIssuesByIds, IEnumerable<IssueDto>>("issues/withIds")
                        .Get<GetIssues, IEnumerable<IssueDto>>("issues/forProject/{projectKey}")
                        .Get<GetEpics, IEnumerable<IssueDto>>("issues/epicsForProject/{projectKey}")
                        .Get<GetIssuesWithoutSprintByProject, IEnumerable<IssueDto>>("issues/backlogForProject/{projectKey}")
                        .Get<GetIssue, IssueDto>("issues/{issueKey}")
                        .Put<UpdateIssue>("issues/{key}")
                        .Delete<DeleteIssue>("issues/{key}")
                        .Post<CreateIssue>("issues",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"issues/{cmd.IssueId}"))
                    ))
                .UseLogging()
                .UseVault();
    }
}
