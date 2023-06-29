using tankman.Models;
using Microsoft.EntityFrameworkCore;

namespace tankman.Db
{
  public class TankmanDbContext : DbContext
  {

    static string? connectionStringFromArgs = null;

    public static void SetConnectionStringFromArgs(string connectionString)
    {
      connectionStringFromArgs = connectionString;
    }

    public DbSet<Org> Orgs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RoleAssignment> RoleAssignments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      #if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
      #endif

      if (TankmanDbContext.connectionStringFromArgs != null)
      {
        optionsBuilder.UseNpgsql(connectionStringFromArgs).UseSnakeCaseNamingConvention();
      }
      else
      {
        var connectionString = Environment.GetEnvironmentVariable("TANKMAN_CONN_STR");
        if (connectionString != null)
        {
          optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

        }
        else
        {
          throw new Exception("The connection string has to be provided in the TANKMAN_CONN_STR env variable or via CLI options.");
        }
      }
    }
  }
}