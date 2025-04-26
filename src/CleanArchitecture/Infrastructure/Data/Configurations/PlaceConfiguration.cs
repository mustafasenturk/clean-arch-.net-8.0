using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Phone).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Manager).IsRequired().HasMaxLength(100);
        
        // Apply same query filter as Tenant to avoid inconsistencies
        builder.HasQueryFilter(p => !p.Tenant.IsDeleted);
        
        // Many-to-one relationship with Tenant
        builder.HasOne(p => p.Tenant)
            .WithMany(t => t.Places)
            .HasForeignKey(p => p.TenantId)
            .IsRequired();
    }
} 