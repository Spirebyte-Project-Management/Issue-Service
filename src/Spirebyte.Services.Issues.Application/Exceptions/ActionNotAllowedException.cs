using Spirebyte.Services.Issues.Application.Exceptions.Base;

namespace Spirebyte.Services.Issues.Application.Exceptions;

public class ActionNotAllowedException : AppException
{
    public ActionNotAllowedException()
        : base("You do not have the permissions to perform this action")
    {
    }

    public override string Code { get; } = "action_not_allowed";
}