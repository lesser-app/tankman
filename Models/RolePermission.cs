using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class RolePermission : IPermission
{
  public string Id { get; set; }
  public Resource Resource { get; set; }
  public Role Role{ get; set; } 
  public string Action { get; set; }
  public DateTime CreatedAt { get; set; }
}