using CleanArchitecture.Application.Common.Models.DTOs;
using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<IEnumerable<TenantListDto>> GetAllTenantsAsync(bool includeDeleted = false)
    {
        var tenants = await _tenantRepository.GetAllAsync(includeDeleted);
        return tenants.Select(t => new TenantListDto
        {
            Id = t.Id,
            Name = t.Name,
            Address = t.Address,
            Type = t.Type,
            Status = t.Status
        });
    }

    public async Task<IEnumerable<TenantListDto>> GetTenantsByStatusAsync(PinStatus status)
    {
        var tenants = await _tenantRepository.GetByStatusAsync(status);
        return tenants.Select(t => new TenantListDto
        {
            Id = t.Id,
            Name = t.Name,
            Address = t.Address,
            Type = t.Type,
            Status = t.Status
        });
    }

    public async Task<TenantDto?> GetTenantByIdAsync(int id, bool includeDeleted = false)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id, includeDeleted);
        if (tenant == null)
            return null;

        return MapToDto(tenant);
    }

    public async Task<TenantDto> CreateTenantAsync(TenantDto tenantDto)
    {
        var tenant = MapToEntity(tenantDto);
        tenant.Status = PinStatus.Pending; // Always start as pending
        
        var createdTenant = await _tenantRepository.CreateAsync(tenant);
        return MapToDto(createdTenant);
    }

    public async Task<TenantDto> UpdateTenantAsync(TenantDto tenantDto)
    {
        if (!tenantDto.Id.HasValue)
            throw new ArgumentException("Tenant ID must be provided for updates");

        var existingTenant = await _tenantRepository.GetByIdAsync(tenantDto.Id.Value);
        if (existingTenant == null)
            throw new KeyNotFoundException($"Tenant with ID {tenantDto.Id} not found");

        // Update tenant properties but preserve status
        var tenant = MapToEntity(tenantDto);
        tenant.Status = existingTenant.Status; // Preserve status
        
        var updatedTenant = await _tenantRepository.UpdateAsync(tenant);
        return MapToDto(updatedTenant);
    }

    public async Task<bool> DeleteTenantAsync(int id)
    {
        return await _tenantRepository.DeleteAsync(id);
    }

    public async Task<bool> ApproveTenantAsync(int id)
    {
        return await _tenantRepository.UpdateStatusAsync(id, PinStatus.Approved);
    }

    public async Task<bool> RejectTenantAsync(int id)
    {
        return await _tenantRepository.UpdateStatusAsync(id, PinStatus.Rejected);
    }

    private static TenantDto MapToDto(Tenant tenant)
    {
        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Address = tenant.Address,
            Type = tenant.Type,
            MainPhone = tenant.MainPhone,
            Lead = tenant.Lead,
            ImageUrl = tenant.ImageUrl,
            Status = tenant.Status,
            Coordinates = new CoordinatesDto
            {
                Latitude = tenant.Coordinates.Latitude,
                Longitude = tenant.Coordinates.Longitude
            },
            Places = tenant.Places.Select(p => new PlaceDto
            {
                Id = p.Id,
                Title = p.Title,
                Phone = p.Phone,
                Manager = p.Manager
            }).ToList()
        };
    }

    private static Tenant MapToEntity(TenantDto dto)
    {
        var tenant = new Tenant
        {
            Name = dto.Name,
            Address = dto.Address,
            Type = dto.Type,
            MainPhone = dto.MainPhone,
            Lead = dto.Lead,
            ImageUrl = dto.ImageUrl,
            Status = dto.Status,
            Coordinates = new Coordinates
            {
                Latitude = dto.Coordinates.Latitude,
                Longitude = dto.Coordinates.Longitude
            }
        };

        if (dto.Id.HasValue)
            tenant.Id = dto.Id.Value;

        tenant.Places = dto.Places.Select(p => new Place
        {
            Id = p.Id ?? 0,
            Title = p.Title,
            Phone = p.Phone,
            Manager = p.Manager
        }).ToList();

        return tenant;
    }
} 