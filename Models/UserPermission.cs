using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class UserPermissionJson
{
  public required string UserId { get; set; }
  public required string ResourceId { get; set; }
  public required string Action { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string OrgId { get; set; }
}

[PrimaryKey(nameof(UserId), nameof(ResourceId), nameof(OrgId))]
public class UserPermission : IPermission, IUserAssociated, IResourceAssociated, IActionAssociated, IOrgAssociated
{
  public required string UserId { get; set; }
  [ForeignKey(nameof(UserId) + "," + nameof(OrgId))]
  public User? User { get; set; }

  public required string ResourceId { get; set; }
  [ForeignKey(nameof(ResourceId) + "," + nameof(OrgId))]
  public Resource? Resource { get; set; }

  public required string Action { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  public static UserPermissionJson ToJson(UserPermission entity)
  {
    return new UserPermissionJson
    {
      UserId = entity.UserId,
      ResourceId = entity.ResourceId,
      Action = entity.Action,
      CreatedAt = entity.CreatedAt,
      OrgId = entity.OrgId
    };
  }
}