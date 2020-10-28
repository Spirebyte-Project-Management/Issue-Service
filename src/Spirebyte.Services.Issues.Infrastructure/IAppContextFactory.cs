using Spirebyte.Services.Issues.Application;

namespace Spirebyte.Services.Issues.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}