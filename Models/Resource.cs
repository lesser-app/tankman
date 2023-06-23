using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Resource
{  
  public required string Id { get; set; }
  public required string Path { get; set; }
  public required string OrgId { get; set; }
  public required DateTimeOffset CreatedAt { get; set; }
  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}