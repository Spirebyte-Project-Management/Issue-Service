using System;
using System.Collections.Generic;
using System.Text;

namespace Spirebyte.Services.Issues.Core.Entities
{
    public class Sprint
    {
        public Sprint(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

    }
}
