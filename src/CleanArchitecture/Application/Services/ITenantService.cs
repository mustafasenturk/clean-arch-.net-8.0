using CleanArchitecture.Application.Common.Models.DTOs;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Services;

public interface ITenantService
{
    Task<IEnumerable<TenantListDto>> GetAllTenantsAsync(bool includeDeleted = false);
    Task<IEnumerable<TenantListDto>> GetTenantsByStatusAsync(PinStatus status);
    Task<TenantDto?> GetTenantByIdAsync(int id, bool includeDeleted = false);
    Task<TenantDto> CreateTenantAsync(TenantDto tenantDto);
    Task<TenantDto> UpdateTenantAsync(TenantDto tenantDto);
    Task<bool> DeleteTenantAsync(int id);
    Task<bool> ApproveTenantAsync(int id);
    Task<bool> RejectTenantAsync(int id);
} 