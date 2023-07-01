using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public static class RoleService
{
  public static async Task<OneOf<List<Role>, Error<string>>> GetRolesAsync(string roleId, string orgId, string? properties, Dictionary<string, string> matchProperties)
  {
    var dbContext = new TankmanDbContext();
    var results = await dbContext.Roles
      .FilterByOrg(orgId)
      .FilterByIdPattern(roleId)
      .FilterByProperties<Role, RoleProperty>(matchProperties)
      .Include(x => x.Properties)
      .ApplyLimit()
      .ToListAsync();

    var shownProps = properties != null ? properties.Split(",") : new string[] { };

    foreach (var result in results)
    {
      result.Properties = result.Properties.Where(x => !x.Hidden || shownProps.Contains(x.Name)).ToList();
    }

    return results;
  }

  public static async Task<OneOf<Role, Error<string>>> CreateRoleAsync(string roleId, string data, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var role = new Role
    {
      Id = roleId,
      Data = data,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };

    dbContext.Roles.Add(role);
    await dbContext.SaveChangesAsync();
    return role;
  }

  public static async Task<OneOf<Role, Error<string>>> UpdateRoleAsync(string roleId, string data, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var role = await dbContext.Roles.SingleAsync(x => x.Id == roleId && x.OrgId == orgId);
    role.Data = data;
    await dbContext.SaveChangesAsync();
    return role;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteRoleAsync(string roleId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var role = await dbContext.Roles.SingleAsync((x) => x.Id == roleId && x.OrgId == orgId);
    dbContext.Roles.Remove(role);
    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<Dictionary<string, string>, Error<string>>> GetPropertiesAsync(string roleId, string orgId, string name)
  {
    var dbContext = new TankmanDbContext();

    var query = dbContext.RoleProperties
      .FilterByOrg(orgId)
      .FilterByRole(roleId)
      .SelectProperties(name);

    var props = await query.ToListAsync();

    return props.ToDictionary();
  }


  public static async Task<OneOf<bool, Error<string>>> UpdatePropertyAsync(string roleId, string name, string value, bool hidden, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.RoleProperties.SingleOrDefaultAsync(x => x.Name == name && x.RoleId == roleId && x.OrgId == orgId);

    if (property != null)
    {
      property.Value = value;
      property.Hidden = hidden;
    }
    else
    {
      var newProperty = new RoleProperty
      {
        Name = name,
        Value = value,
        Hidden = hidden,
        RoleId = roleId,
        OrgId = orgId,
        CreatedAt = DateTime.UtcNow
      };
      dbContext.RoleProperties.Add(newProperty);
    }

    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<bool, Error<string>>> DeletePropertyAsync(string roleId, string name, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.RoleProperties.SingleOrDefaultAsync(x => x.Name == name && x.RoleId == roleId && x.OrgId == orgId);

    if (property != null)
    {
      dbContext.RoleProperties.Remove(property);
      await dbContext.SaveChangesAsync();
    }

    return true;
  }
}
