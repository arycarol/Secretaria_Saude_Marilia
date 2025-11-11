using CareMove.Infrastructure.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareMove.Infrastructure.Map;

public class VehicleMap : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicle");
        builder.HasKey(x => x.Id).HasName("id");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.LicensePlate)
            .IsRequired()
            .HasMaxLength(15);
        builder.Property(u => u.Color)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.VehicleCategory)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.VehicleModel)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.VehicleFuelType)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.VehicleStatus)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.Renavam)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.Year)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.Capacity)
            .IsRequired();
    }
}