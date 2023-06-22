namespace tankman.Models;

public interface IPermission
{
  public string Resource { get; set; }
  public string Action { get; set; }
  public DateTime CreatedAt { get; set; }
}