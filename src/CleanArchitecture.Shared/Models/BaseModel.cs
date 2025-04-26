namespace CleanArchitecture.Shared.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
}
