using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class RoleAssignment
{
  public required string Id { get; set; }
  public required string UserId { get; set; }
  public required string RoleId { get; set; }
  public required DateTime CreatedAt { get; set; }
}