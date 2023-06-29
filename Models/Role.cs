using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id), nameof(OrgId))]
public class Role : IEntity, IOrgAssociated
{
  public required string Id { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; }

  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}