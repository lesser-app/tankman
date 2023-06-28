using tankman.Http;
using tankman.Models;
using tankman.Services;
using tankman.Types;

namespace tankman.RequestHandlers;

public class CreateRole
{
  public required string Id { get; set; }
}

public static class RoleHandlers
{
  public static async Task<IResult> GetRolesAsync(string orgId)
  {
    return ApiResult.ToResult(await RoleService.GetRolesAsync(orgId));
  }

  public static async Task<IResult> CreateRoleAsync(string orgId, CreateRole createRole)
  {
    return ApiResult.ToResult(await RoleService.CreateRoleAsync(createRole.Id, orgId));
  }
}