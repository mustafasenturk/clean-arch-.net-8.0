using CleanArchitecture.Shared.Models;

namespace CleanArchitecture.Domain.Entities;

public class Tenant : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string MainPhone { get; set; } = string.Empty;
    public string Lead { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Approval status
    public PinStatus Status { get; set; } = PinStatus.Pending;

    // Navigation properties
    public Coordinates Coordinates { get; set; } = null!;
    public ICollection<Place> Places { get; set; } = new List<Place>();
}

public enum PinStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
} 