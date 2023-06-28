using tankman.Models;
using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateRole
{
  public required string Name { get; set; }
}

public static class RoleHandlers
{
  public static async Task<List<Role>> GetRolesAsync(string orgId)
  {
    return await RoleService.GetRolesAsync(orgId);
  }

  public static async Task<Role> CreateRoleAsync(string orgId, CreateRole createRole)
  {
    return await RoleService.CreateRoleAsync(createRole.Name, orgId);
  }
}