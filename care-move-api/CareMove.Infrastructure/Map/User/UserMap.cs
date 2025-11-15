using CareMove.Infrastructure.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareMove.Infrastructure.Map;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id).HasName("id");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.CPF)
            .IsRequired()
            .HasMaxLength(11);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.Telephone)
            .HasMaxLength(20);
        builder.Property(u => u.BirthDate)
            .IsRequired();
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.UserCategory)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.VehicleId);

        builder.HasOne(u => u.Vehicle)
               .WithMany()
               .HasForeignKey(u => u.VehicleId);
    }
}