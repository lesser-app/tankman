using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(RoleId), nameof(ResourceId), nameof(OrgId))]
public class RolePermission : IPermission, IRoleAssociated, IResourceAssociated, IActionAssociated, IOrgAssociated
{
  public required string RoleId { get; set; }
  [ForeignKey(nameof(RoleId) + "," + nameof(OrgId))]
  public Role? Role { get; set; }

  public required string ResourceId { get; set; }
  [ForeignKey(nameof(ResourceId) + "," + nameof(OrgId))]
  public Resource? Resource { get; set; }

  public required string Action { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

}