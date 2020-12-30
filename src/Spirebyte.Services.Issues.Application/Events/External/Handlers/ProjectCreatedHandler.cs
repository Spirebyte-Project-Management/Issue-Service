using Convey.CQRS.Events;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Events.External.Handlers
{
    public class ProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        private readonly IProjectRepository _projectRepository;


        public ProjectCreatedHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task HandleAsync(ProjectCreated @event)
        {
            if (await _projectRepository.ExistsAsync(@event.ProjectId))
            {
                throw new ProjectAlreadyCreatedException(@event.ProjectId);
            }

            var project = new Project(@event.ProjectId);
            await _projectRepository.AddAsync(project);
        }
    }
}
