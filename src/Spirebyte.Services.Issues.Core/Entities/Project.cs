using System;

namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }


        public Project(Guid id, string key)
        {
            Id = id;
            Key = key;
        }
    }
}