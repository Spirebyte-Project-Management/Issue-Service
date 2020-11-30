using Convey.CQRS.Events;
using System.Threading.Tasks;

namespace Spirebyte.Services.Issues.Application.Services.Interfaces
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}
