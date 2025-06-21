using ecotrip_backend.Auth.Domain.Aggregates;
using ecotrip_backend.Auth.Domain.Repositories.Interfaces;

namespace ecotrip_backend.Auth.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _users = new();

    public Task<User?> GetByEmailAsync(string email)
    {
        _users.TryGetValue(email.ToLowerInvariant(), out var user);
        return Task.FromResult(user);
    }

    public Task CreateAsync(User user)
    {
        _users[user.Email] = user;
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string email)
    {
        return Task.FromResult(_users.ContainsKey(email.ToLowerInvariant()));
    }
}
