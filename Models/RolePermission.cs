namespace tankman.Models;

public class RolePermission : IPermission
{
  public required string Resource { get; set; }
  public required string Role { get; set; }
  public required string Action { get; set; }
  public required DateTime CreatedAt { get; set; }
}