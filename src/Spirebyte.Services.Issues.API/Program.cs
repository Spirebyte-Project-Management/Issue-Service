using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework;
using Spirebyte.Framework.Auth;
using Spirebyte.Services.Issues.Application;
using Spirebyte.Services.Issues.Core.Constants;
using Spirebyte.Services.Issues.Infrastructure;

namespace Spirebyte.Services.Issues.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateWebHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => services
                .AddApplication()
                .AddInfrastructure(ctx.Configuration)
                .Configure<AuthorizationOptions>(options =>
                {
                    options.AddEitherOrScopePolicy(ApiScopes.IssuesRead, ApiScopes.IssuesRead, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssuesWrite, ApiScopes.IssuesWrite, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssuesDelete, ApiScopes.IssuesDelete, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssueCommentsRead, ApiScopes.IssueCommentsRead, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssueCommentsWrite, ApiScopes.IssueCommentsWrite, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssueCommentsDelete, ApiScopes.IssueCommentsDelete, ApiScopes.IssuesManage);
                    options.AddEitherOrScopePolicy(ApiScopes.IssueHistoryRead, ApiScopes.IssueHistoryRead, ApiScopes.IssuesManage);
                })
                .AddControllers()
            )
            .Configure(app => app
                .UseSpirebyteFramework()
                .UseApplication()
                .UseInfrastructure()
                .UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("",
                            ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppInfo>().Name));
                        endpoints.MapGet("/ping", () => "pong");
                        endpoints.MapControllers();
                    }
                ))
            .AddSpirebyteFramework();
    }
}