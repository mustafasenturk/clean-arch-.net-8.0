using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Models.DTOs;

public class CoordinatesDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class PlaceDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
}

public class TenantDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string MainPhone { get; set; } = string.Empty;
    public string Lead { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public PinStatus Status { get; set; } = PinStatus.Pending;
    public CoordinatesDto Coordinates { get; set; } = new();
    public List<PlaceDto> Places { get; set; } = new();
}

public class TenantListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public PinStatus Status { get; set; }
} 