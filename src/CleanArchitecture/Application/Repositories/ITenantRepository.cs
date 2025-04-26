using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Repositories;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync(bool includeDeleted = false);
    Task<IEnumerable<Tenant>> GetByStatusAsync(PinStatus status);
    Task<Tenant?> GetByIdAsync(int id, bool includeDeleted = false);
    Task<Tenant> CreateAsync(Tenant tenant);
    Task<Tenant> UpdateAsync(Tenant tenant);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStatusAsync(int id, PinStatus status);
} 