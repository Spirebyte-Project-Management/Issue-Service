using System;
using System.Threading.Tasks;
using Spirebyte.Services.Issues.Core.Entities;

namespace Spirebyte.Services.Issues.Core.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(User user);
}