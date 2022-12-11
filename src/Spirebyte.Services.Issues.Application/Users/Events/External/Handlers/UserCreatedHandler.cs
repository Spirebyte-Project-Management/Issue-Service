using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Issues.Application.Exceptions;
using Spirebyte.Services.Issues.Core.Entities;
using Spirebyte.Services.Issues.Core.Repositories;

namespace Spirebyte.Services.Issues.Application.Users.Events.External.Handlers;

public class UserCreatedHandler : IEventHandler<UserCreated>
{
    private readonly IUserRepository _userRepository;
    
    public UserCreatedHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(UserCreated @event, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsAsync(@event.UserId)) throw new UserAlreadyCreatedException(@event.UserId);

        var user = new User(@event.UserId);
        await _userRepository.AddAsync(user);
    }
}