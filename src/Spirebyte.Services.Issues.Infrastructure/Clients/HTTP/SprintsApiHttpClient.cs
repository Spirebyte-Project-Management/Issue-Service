using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.HTTP;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;

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

        public Task<bool> IsProjectUserAsync(string key, Guid userId) => _client.GetAsync<bool>($"{_url}/projects/{key}/hasuser/{userId}/");

        public Task<Guid[]> IssuesWithoutSprintForProject(string projectKey) => _client.GetAsync<Guid[]>($"{_url}/issuesWithoutSprintForProject/{projectKey}");
    }
}
