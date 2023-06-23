using tankman.Models;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;

namespace tankman.Db
{
  public class TankmanDbContext : DbContext
  {
    public DbSet<Org> Orgs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RoleAssignment> RoleAssignments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var connectionString = Environment.GetEnvironmentVariable("CONN_STR");
      optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
    }
  }

}