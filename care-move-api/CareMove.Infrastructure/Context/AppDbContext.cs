using CareMove.Infrastructure.Entity.Entity;
using CareMove.Infrastructure.Map;
using Microsoft.EntityFrameworkCore;

namespace CareMove.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new VehicleMap());
        modelBuilder.ApplyConfiguration(new TransportRequestMap());
        modelBuilder.ApplyConfiguration(new TransportAssignmentMap());
    }
    public DbSet<User> User { get; set; }
    public DbSet<Vehicle> Vehicle { get; set; }
    public DbSet<TransportRequest> TransportRequest { get; set; }
    public DbSet<TransportAssignment> TransportAssignment { get; set; }
}