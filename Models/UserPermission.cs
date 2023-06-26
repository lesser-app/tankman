using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class UserPermission : IPermission
{
  public string Id { get; set; }
  public string UserId { get; set; }
  public string Resource { get; set; }
  public string Action { get; set; }
  public DateTime CreatedAt { get; set; }
}