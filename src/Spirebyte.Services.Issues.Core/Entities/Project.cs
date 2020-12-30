namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Project
    {
        public string Id { get; private set; }


        public Project(string id)
        {
            Id = id;
        }
    }
}