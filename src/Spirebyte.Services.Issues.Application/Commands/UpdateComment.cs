using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Issues.Application.Commands
{
    [Contract]
    public class UpdateComment : ICommand
    {
        public string Id { get; private set; }
        public string Body { get; private set; }

        public UpdateComment(string id, string body)
        {
            Id = id;
            Body = body;
        }
    }
}
