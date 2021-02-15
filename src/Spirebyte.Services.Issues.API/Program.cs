using Convey;
using Convey.CQRS.Queries;
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
using Spirebyte.Services.Issues.Core.Repositories;
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
                        .Get<GetComments, IEnumerable<CommentDto>>("issues/comments")
                        .Get<GetComment, CommentDto>("issues/comment/{id}")
                        .Post<CreateIssue>("issues/comment",
                            afterDispatch: async (cmd, ctx) =>
                            {
                                var comment = await ctx.RequestServices.GetService<ICommentRepository>().GetLatest();
                                await ctx.Response.Created($"issues/comment/{comment.Id}",
                                    await ctx.RequestServices.GetService<IQueryDispatcher>()
                                        .QueryAsync<GetComment, CommentDto>(new GetComment(comment.Id)));
                            })
                        .Get<GetIssues, IEnumerable<IssueDto>>("issues")
                        .Get<GetIssue, IssueDto>("issues/{id}")
                        .Put<UpdateIssue>("issues/{IssueId}")
                        .Delete<DeleteIssue>("issues/{IssueId}")
                        .Post<CreateIssue>("issues",
                            afterDispatch: async (cmd, ctx) =>
                            {
                                var issue = await ctx.RequestServices.GetService<IIssueRepository>().GetLatest();
                                await ctx.Response.Created($"issues/{issue.Id}",
                                    await ctx.RequestServices.GetService<IQueryDispatcher>()
                                        .QueryAsync<GetIssue, IssueDto>(new GetIssue(issue.Id)));
                            })

                    ))
                .UseLogging()
                .UseVault();
    }
}
