namespace tankman.Models;

public class UserPermission
{
  public required string UserId { get; set; }
  public required string Resource { get; set; }
  public required string Action { get; set; }
  public required string OrgId { get; set; } 
  public required DateTime CreatedAt { get; set; }
}