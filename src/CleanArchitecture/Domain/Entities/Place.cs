using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Place
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
    
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
} 