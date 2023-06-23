using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;

namespace tankman.Services;

public static class ResourceService
{
  public async static Task<List<Resource>> GetResourcesAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Resources.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public async static Task<List<IPermission>> GetPermissionsAsync(string resourcePath, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var resource = await dbContext.Resources.SingleAsync((x) => x.Path == resourcePath && x.OrgId == orgId);
    var result = new List<IPermission>();
    result.AddRange(resource.UserPermissions);
    result.AddRange(resource.RolePermissions);
    return result;
  }
}
