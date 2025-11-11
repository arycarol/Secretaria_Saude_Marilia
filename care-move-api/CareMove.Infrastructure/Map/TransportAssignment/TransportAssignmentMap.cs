using CareMove.Infrastructure.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareMove.Infrastructure.Map;

public class TransportAssignmentMap : IEntityTypeConfiguration<TransportAssignment>
{
    public void Configure(EntityTypeBuilder<TransportAssignment> builder)
    {
        builder.ToTable("transport_assignment");
        builder.HasKey(x => x.Id).HasName("id");

        builder.Property(u => u.Date)
            .HasMaxLength(50);
        builder.Property(u => u.TransportAssignmentStatus)
            .HasMaxLength(50);
        builder.Property(u => u.VehicleId)
            .IsRequired();
        builder.Property(u => u.DriverUserId)
            .IsRequired();
        builder.Property(u => u.PacientUserId)
            .IsRequired();
        builder.Property(u => u.TransportRequestId)
            .IsRequired();

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(u => u.VehicleId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(u => u.DriverUserId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(u => u.PacientUserId);

        builder.HasOne<TransportRequest>()
            .WithMany()
            .HasForeignKey(u => u.TransportRequestId);
    }
}