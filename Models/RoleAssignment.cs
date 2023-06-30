using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class RoleAssignmentJson
{
  public required string UserId { get; set; }
  public required string RoleId { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string OrgId { get; set; }
}

[PrimaryKey(nameof(RoleId), nameof(UserId), nameof(OrgId))]
public class RoleAssignment : IUserAssociated, IRoleAssociated, IOrgAssociated
{
  public required string UserId { get; set; }
  [ForeignKey(nameof(UserId) + "," + nameof(OrgId))]
  public User? User { get; set; }

  public required string RoleId { get; set; }
  [ForeignKey(nameof(RoleId) + "," + nameof(OrgId))]
  public Role? Role { get; set; }

  public DateTime CreatedAt { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  public static RoleAssignmentJson ToJson(RoleAssignment entity)
  {
    return new RoleAssignmentJson
    {
      RoleId = entity.RoleId,
      UserId = entity.UserId,
      CreatedAt = entity.CreatedAt,
      OrgId = entity.OrgId
    };
  }
}