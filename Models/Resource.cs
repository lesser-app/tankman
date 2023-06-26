using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Resource
{  
  public string Id { get; set; }
  public string Path { get; set; }
  public string OrgId { get; set; }
  public DateTime CreatedAt { get; set; }
  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}