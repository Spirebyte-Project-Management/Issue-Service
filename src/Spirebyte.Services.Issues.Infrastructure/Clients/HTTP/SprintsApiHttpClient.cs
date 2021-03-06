using Convey.HTTP;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Infrastructure.Clients.HTTP
{
    internal sealed class SprintsApiHttpClient : ISprintsApiHttpClient
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public SprintsApiHttpClient(IHttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["sprints"];
        }

        public Task<string[]> IssuesWithoutSprintForProject(string projectId) => _client.GetAsync<string[]>($"{_url}/issuesWithoutSprintForProject/{projectId}");
    }
}
