using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Address).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Type).IsRequired().HasMaxLength(50);
        builder.Property(t => t.MainPhone).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Lead).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Status).IsRequired();
        
        // Soft delete configuration
        builder.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.HasQueryFilter(t => !t.IsDeleted);
        
        // Coordinates are owned by Tenant
        builder.OwnsOne(t => t.Coordinates, coordinates =>
        {
            coordinates.Property(c => c.Latitude).IsRequired();
            coordinates.Property(c => c.Longitude).IsRequired();
        });
        
        // One-to-many relationship with Places
        builder.HasMany(t => t.Places)
            .WithOne(p => p.Tenant)
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 