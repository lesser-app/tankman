using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Role
{
  public string Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public Org Org { get; set; }
  public List<RoleAssignment> RoleAssignments { get; set; }
  public List<RolePermission> RolePermissions { get; set; }
}