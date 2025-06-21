using System.Threading.Tasks;

namespace ecotrip_backend.Tourists.Domain;

public interface ITouristRepository
{
    Task<Tourist?> GetByEmailAsync(string email);
    Task<Tourist?> GetByIdAsync(Guid id);
    Task<IEnumerable<Tourist>> GetAllAsync();
    Task AddAsync(Tourist tourist);
    Task UpdateAsync(Tourist tourist);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}