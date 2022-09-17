using Bogus;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Tests.Shared.MockData.Entities;

public sealed class ProjectFaker : Faker<Project>
{
    private ProjectFaker()
    {
        CustomInstantiator(_ => new Project(default));
        RuleFor(r => r.Id, f => f.Random.Word());
    }

    public static ProjectFaker Instance => new();
}