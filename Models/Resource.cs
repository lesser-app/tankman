using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class ResourceJson
{
  public required string Id { get; set; }
  public required string Data { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string OrgId { get; set; }
}

[PrimaryKey(nameof(Id), nameof(OrgId))]
public class Resource : IPathedEntity, IOrgAssociated
{
  public required string Id { get; set; }

  public required string Data { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  public ResourcePath? ResourcePath { get; set; }

  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

  public static ResourceJson ToJson(Resource resource)
  {
    return new ResourceJson
    {
      Id = resource.Id,
      CreatedAt = resource.CreatedAt,
      Data = resource.Data,
      OrgId = resource.OrgId
    };
  }
}