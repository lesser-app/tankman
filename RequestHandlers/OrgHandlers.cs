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
  public static async Task<List<Models.Org>> GetOrgs()
  {
    return await OrgService.GetOrgsAsync();
  }

  public static async Task<Models.Org> CreateOrg(CreateOrg org)
  {
    return await OrgService.CreateOrgAsync(org.Id);
  }

  public static async Task<Models.Org> GetOrg(string id)
  {
    return await OrgService.GetOrgAsync(id);
  }

  public static async Task<Models.Org> PatchOrg(string id, PatchOrg patchOrg)
  {
    if (patchOrg.Op.ToLower() == "activate")
    {
      return await OrgService.ActivateOrgAsync(id);
    }
    else if (patchOrg.Op.ToLower() == "deactivate")
    {
      return await OrgService.DeactivateOrgAsync(id);
    }
    else
    {
      throw new Exception("Invalid operation on org.");
    }
  }

}