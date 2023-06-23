using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class UserPermission : IPermission
{
  public required string Id { get; set; }
  public required string UserId { get; set; }
  public required string Resource { get; set; }
  public required string Action { get; set; }
  public required DateTime CreatedAt { get; set; }
}