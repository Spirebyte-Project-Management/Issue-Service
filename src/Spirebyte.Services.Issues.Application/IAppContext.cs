namespace Spirebyte.Services.Issues.Application;

public interface IAppContext
{
    string RequestId { get; }
    IIdentityContext Identity { get; }
}