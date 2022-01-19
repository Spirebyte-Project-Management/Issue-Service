using System;

namespace Spirebyte.Services.Issues.Core.Entities;

public class User
{
    public User(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}