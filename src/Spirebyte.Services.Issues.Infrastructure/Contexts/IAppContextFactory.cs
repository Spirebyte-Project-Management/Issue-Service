using Spirebyte.Services.Issues.Application.Contexts;

namespace Spirebyte.Services.Issues.Infrastructure;

public interface IAppContextFactory
{
    IAppContext Create();
}