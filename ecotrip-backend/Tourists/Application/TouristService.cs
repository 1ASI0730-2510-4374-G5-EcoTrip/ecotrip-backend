using ecotrip_backend.Tourists.Application.DTOs;
using ecotrip_backend.Tourists.Domain;

namespace ecotrip_backend.Tourists.Application;

public class TouristService
{
    private readonly ITouristRepository _repo;

    public TouristService(ITouristRepository repo)
    {
        _repo = repo;
    }

    public async Task<TouristProfileDto> RegisterAsync(RegisterTouristDto dto)
    {
        var existing = await _repo.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            throw new Exception("El email ya esta registrado.");
        }

        var tourist = new Tourist(dto.FullName, dto.Email, dto.Country);
        await _repo.AddAsync(tourist);
        return MapToProfileDto(tourist);
    }
    
    public async Task<TouristProfileDto?> GetProfileByIdAsync(Guid id)
    {
        var tourist = await _repo.GetByIdAsync(id);
        if (tourist == null)
            return null;
            
        return MapToProfileDto(tourist);
    }
    
    public async Task<IEnumerable<TouristProfileDto>> GetAllProfilesAsync()
    {
        var tourists = await _repo.GetAllAsync();
        return tourists.Select(MapToProfileDto);
    }
    
    public async Task<TouristProfileDto?> UpdateProfileAsync(Guid id, UpdateTouristProfileDto dto)
    {
        var tourist = await _repo.GetByIdAsync(id);
        if (tourist == null)
            return null;
            
        tourist.UpdateProfile(dto.FullName, dto.Country, dto.Bio, dto.ProfilePictureUrl);
        await _repo.UpdateAsync(tourist);
        
        return MapToProfileDto(tourist);
    }
    
    public async Task<bool> DeleteProfileAsync(Guid id)
    {
        if (!await _repo.ExistsByIdAsync(id))
            return false;
            
        await _repo.DeleteAsync(id);
        return true;
    }
    
    private TouristProfileDto MapToProfileDto(Tourist tourist)
    {
        return new TouristProfileDto
        {
            Id = tourist.Id,
            FullName = tourist.FullName,
            Email = tourist.Email,
            Country = tourist.Country,
            Bio = tourist.Bio,
            ProfilePictureUrl = tourist.ProfilePictureUrl,
            CreatedAt = tourist.CreatedAt,
            UpdatedAt = tourist.UpdatedAt
        };
    }
}