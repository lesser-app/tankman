using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class UserJson
{
  public required string Id { get; set; }
  public required string Data { get; set; }
  public required string IdentityProvider { get; set; }
  public required string IdentityProviderUserId { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string OrgId { get; set; }
  public List<string> Roles { get; set; } = new List<string>();
}


[PrimaryKey(nameof(Id), nameof(OrgId))]
public class User : IEntity, IOrgAssociated
{
  public required string Id { get; set; }

  public required string Data { get; set; }

  public required string IdentityProvider { get; set; }

  public required string IdentityProviderUserId { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  [NotMapped]
  public List<string> Roles { get; set; } = new List<string>();

  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

  public static UserJson ToJson(User user)
  {
    return new UserJson
    {
      Id = user.Id,
      Data = user.Data,
      IdentityProvider = user.IdentityProvider,
      IdentityProviderUserId = user.IdentityProviderUserId,
      CreatedAt = user.CreatedAt,
      OrgId = user.OrgId,
      Roles = user.Roles
    };
  }
}