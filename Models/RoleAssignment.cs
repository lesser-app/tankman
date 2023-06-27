using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class RoleAssignment
{
  public string Id { get; set; }
  public User User { get; set; }
  public Role Role { get; set; }
  public DateTime CreatedAt { get; set; }
}