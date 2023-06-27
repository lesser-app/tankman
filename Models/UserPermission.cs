using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class UserPermission : IPermission
{
  public string Id { get; set; }
  public User User { get; set; }
  public Resource Resource { get; set; }
  public string Action { get; set; }
  public DateTime CreatedAt { get; set; }
}