using CareMove.Infrastructure.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareMove.Infrastructure.Map;

public class TransportRequestMap : IEntityTypeConfiguration<TransportRequest>
{
    public void Configure(EntityTypeBuilder<TransportRequest> builder)
    {
        builder.ToTable("transport_request");
        builder.HasKey(x => x.Id).HasName("id");

        builder.Property(u => u.OriginLocation)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.DestinationLocation)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.Date)
            .IsRequired();
        builder.Property(u => u.Hour)
            .IsRequired();
        builder.Property(u => u.UserId)
            .IsRequired();
        builder.Property(u => u.TransportKind)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.TransportStatus)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(u => u.UserId);
    }
}