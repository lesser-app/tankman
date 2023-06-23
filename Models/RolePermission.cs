using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class RolePermission : IPermission
{
  public required string Id { get; set; }
  public required string Resource { get; set; }
  public required string Role { get; set; }
  public required string Action { get; set; }
  public required DateTimeOffset CreatedAt { get; set; }
}