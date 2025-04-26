using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly ApplicationDbContext _context;

    public TenantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync(bool includeDeleted = false)
    {
        IQueryable<Tenant> query = _context.Tenants
            .Include(t => t.Places);

        if (includeDeleted)
        {
            // Remove the global query filter for IsDeleted
            query = query.IgnoreQueryFilters();
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Tenant>> GetByStatusAsync(PinStatus status)
    {
        return await _context.Tenants
            .Include(t => t.Places)
            .Where(t => t.Status == status)
            .ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        IQueryable<Tenant> query = _context.Tenants
            .Include(t => t.Places);

        if (includeDeleted)
        {
            // Remove the global query filter for IsDeleted
            query = query.IgnoreQueryFilters();
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tenant> CreateAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task<Tenant> UpdateAsync(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
            return false;

        // Soft delete
        tenant.IsDeleted = true;
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, PinStatus status)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
            return false;

        tenant.Status = status;
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
        return true;
    }
} 