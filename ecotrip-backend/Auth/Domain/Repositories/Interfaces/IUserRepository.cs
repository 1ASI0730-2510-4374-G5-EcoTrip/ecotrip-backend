using ecotrip_backend.Auth.Domain.Aggregates;

namespace ecotrip_backend.Auth.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task CreateAsync(User user);
    Task<bool> ExistsAsync(string email);
}
