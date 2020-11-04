using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.HTTP;
using Spirebyte.Services.Issues.Application.Clients.Interfaces;

namespace Spirebyte.Services.Issues.Infrastructure.Clients.HTTP
{
    internal sealed class ProjectsApiHttpClient : IProjectsApiHttpClient
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public ProjectsApiHttpClient(IHttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["projects"];
        }

        public Task<bool> IsProjectUserAsync(string key, Guid userId) => _client.GetAsync<bool>($"{_url}/projects/{key}/hasuser/{userId}/");
    }
}
