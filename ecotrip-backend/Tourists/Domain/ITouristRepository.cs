using System.Threading.Tasks;

namespace backendPrueva.Tourists.Domain;

public interface ITouristRepository
{
    Task<Tourist?> GetByEmailAsync(string email);
    Task AddAsync(Tourist tourist);
}