using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace tankman.Models;

[PrimaryKey(nameof(Name), nameof(RoleId), nameof(OrgId))]
public class RoleProperty : DynamicProperty, IRoleAssociated, IOrgAssociated
{
  public required string RoleId { get; set; }
  [ForeignKey(nameof(RoleId) + "," + nameof(OrgId))]
  public Role? Role { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;
}