using backendPrueva.Tourists.Application.DTos;
using backendPrueva.Tourists.Domain;

namespace backendPrueva.Tourists.Application;

public class TouristService
{
    private readonly ITouristRepository _repo;

    public TouristService(ITouristRepository repo)
    {
        _repo = repo;
    }

    public async Task<Tourist> RegisterAsync(RegisterTouristDto dto)
    {
        var existing = await _repo.GetByEmailAsync(dto.Email);
        if (existing != null)
        {
            throw new Exception("El email ya esta registrado.");
        }

        var tourist = new Tourist(dto.FullName, dto.Email, dto.Country);
        await _repo.AddAsync(tourist);
        return tourist;
    }
}