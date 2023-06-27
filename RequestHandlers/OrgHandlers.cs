using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateOrg
{
  public string Id { get; set; }
}

public class PatchOrg
{
  public string Op { get; set; }
}

public static class OrgHandlers
{
  public static async Task<List<Models.Org>> GetOrgsAsync()
  {
    return await OrgService.GetOrgsAsync();
  }

  public static async Task<Models.Org> CreateOrgAsync(CreateOrg org)
  {
    return await OrgService.CreateOrgAsync(org.Id);
  }

  public static async Task<Models.Org> GetOrgAsync(string orgId)
  {
    return await OrgService.GetOrgAsync(orgId);
  }

  public static async Task<Models.Org> PatchOrgAsync(string orgId, PatchOrg patchOrg)
  {
    if (patchOrg.Op.ToLower() == "activate")
    {
      return await OrgService.ActivateOrgAsync(orgId);
    }
    else if (patchOrg.Op.ToLower() == "deactivate")
    {
      return await OrgService.DeactivateOrgAsync(orgId);
    }
    else
    {
      throw new Exception("Invalid operation on org.");
    }
  }

}