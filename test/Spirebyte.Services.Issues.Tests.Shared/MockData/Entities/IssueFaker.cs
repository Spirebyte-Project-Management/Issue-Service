using System.Runtime.Serialization;
using Bogus;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Tests.Shared.MockData.Entities;

public sealed class IssueFaker : Faker<Issue>
{
    private IssueFaker()
    {
        CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(Issue)) as Issue);
        RuleFor(r => r.Id, f => f.Random.Word());
        RuleFor(r => r.Type, f => f.Random.Enum<IssueType>());
        RuleFor(r => r.Status, f => f.Random.Enum<IssueStatus>());
        RuleFor(r => r.Title, f => f.Commerce.ProductName());
        RuleFor(r => r.Description, f => f.Commerce.ProductDescription());
        RuleFor(r => r.StoryPoints, f => f.Random.Number());
        RuleFor(r => r.ProjectId, f => f.Random.Word());
        RuleFor(r => r.CreatedAt, f => f.Date.Past());
    }

    public static IssueFaker Instance => new();
}