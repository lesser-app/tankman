namespace tankman.Models;

public class Resource
{
  public required string Path { get; set; }
  public required string OrgId { get; set; }
  public required DateTime CreatedAt { get; set; }
  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}