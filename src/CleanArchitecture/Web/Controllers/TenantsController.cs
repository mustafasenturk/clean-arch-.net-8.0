using CleanArchitecture.Application.Common.Models.DTOs;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    // GET: api/tenants
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantListDto>>> GetAllTenants([FromQuery] bool includeDeleted = false)
    {
        var tenants = await _tenantService.GetAllTenantsAsync(includeDeleted);
        return Ok(tenants);
    }

    // GET: api/tenants/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<TenantListDto>>> GetPendingTenants()
    {
        var tenants = await _tenantService.GetTenantsByStatusAsync(PinStatus.Pending);
        return Ok(tenants);
    }

    // GET: api/tenants/approved
    [HttpGet("approved")]
    public async Task<ActionResult<IEnumerable<TenantListDto>>> GetApprovedTenants()
    {
        var tenants = await _tenantService.GetTenantsByStatusAsync(PinStatus.Approved);
        return Ok(tenants);
    }

    // GET: api/tenants/rejected
    [HttpGet("rejected")]
    public async Task<ActionResult<IEnumerable<TenantListDto>>> GetRejectedTenants()
    {
        var tenants = await _tenantService.GetTenantsByStatusAsync(PinStatus.Rejected);
        return Ok(tenants);
    }

    // GET: api/tenants/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(int id)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(id);
        if (tenant == null)
            return NotFound();

        return Ok(tenant);
    }

    // POST: api/tenants
    [HttpPost]
    public async Task<ActionResult<TenantDto>> CreateTenant(TenantDto tenantDto)
    {
        var createdTenant = await _tenantService.CreateTenantAsync(tenantDto);
        return CreatedAtAction(nameof(GetTenant), new { id = createdTenant.Id }, createdTenant);
    }

    // PUT: api/tenants/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Only admins can update
    public async Task<ActionResult<TenantDto>> UpdateTenant(int id, TenantDto tenantDto)
    {
        if (!tenantDto.Id.HasValue)
            tenantDto.Id = id;
        else if (tenantDto.Id != id)
            return BadRequest("ID mismatch");

        try
        {
            var updatedTenant = await _tenantService.UpdateTenantAsync(tenantDto);
            return Ok(updatedTenant);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/tenants/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only admins can delete
    public async Task<IActionResult> DeleteTenant(int id)
    {
        var result = await _tenantService.DeleteTenantAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // POST: api/tenants/5/approve
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin")] // Only admins can approve
    public async Task<IActionResult> ApproveTenant(int id)
    {
        var result = await _tenantService.ApproveTenantAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // POST: api/tenants/5/reject
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin")] // Only admins can reject
    public async Task<IActionResult> RejectTenant(int id)
    {
        var result = await _tenantService.RejectTenantAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 