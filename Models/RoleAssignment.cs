using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class RoleAssignment
{
  public string Id { get; set; }
  public string UserId { get; set; }
  public string RoleId { get; set; }
  public DateTime CreatedAt { get; set; }
}