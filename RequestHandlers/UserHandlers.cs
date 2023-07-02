using tankman.Http;
using tankman.Utils;
using tankman.Services;
using tankman.Models;

namespace tankman.RequestHandlers;

public class CreateUser
{
  public required string Id { get; set; }
  public required string IdentityProviderUserId { get; set; }
  public required string IdentityProvider { get; set; }
  public required string Data { get; set; }
}

public class UpdateUser
{
  public required string IdentityProviderUserId { get; set; }
  public required string IdentityProvider { get; set; }
  public required string Data { get; set; }
}


public class AssignRole
{
  public required string RoleId { get; set; }
}

public static class UserHandlers
{
  public static async Task<IResult> GetUsersAsync(string? userId, string orgId, string? properties, string? sort, string? order, int? from, int? limit, HttpContext context)
  {
    var matchProperties = QueryStringUtils.GetPrefixedQueryDictionary("properties.", context);

    SortUserBy? sortBy = sort switch
    {
      "id" => SortUserBy.UserId,
      "createdAt" => SortUserBy.CreatedAt,
      _ => null
    };

    SortOrder? sortOrder = order switch
    {
      "asc" => SortOrder.Ascending,
      "desc" => SortOrder.Descending,
      _ => null
    };

    return ApiResult.ToResult(
      await UserService.GetUsersAsync(
        userId: userId ?? Settings.Wildcard,
        orgId: orgId,
        properties: properties,
        sortBy: sortBy,
        sortOrder: sortOrder,
        from: from,
        limit: limit,
        matchProperties: matchProperties
      ),
      User.ToJson
    );
  }

  public static async Task<IResult> CreateUserAsync(string orgId, CreateUser create)
  {
    return ApiResult.ToResult(
      await UserService.CreateUserAsync(
        userId: create.Id,
        identityProviderUserId: create.IdentityProviderUserId,
        identityProvider: create.IdentityProvider,
        data: create.Data,
        orgId: orgId
      ),
      User.ToJson
    );
  }

  public static async Task<IResult> UpdateUserAsync(string userId, string orgId, UpdateUser update)
  {
    return ApiResult.ToResult(
      await UserService.UpdateUserAsync(
        userId: userId,
        identityProviderUserId: update.IdentityProviderUserId,
        identityProvider: update.IdentityProvider,
        data: update.Data,
        orgId: orgId
      ),
      User.ToJson
    );
  }


  public static async Task<IResult> AssignRoleAsync(string userId, string orgId, AssignRole assign)
  {
    return ApiResult.ToResult(
      await UserService.AssignRoleAsync(
        roleId: assign.RoleId,
        userId: userId,
        orgId: orgId
      ),
      RoleAssignment.ToJson
    );
  }

  public static async Task<IResult> UnassignRoleAsync(string roleId, string userId, string orgId)
  {
    return ApiResult.ToResult(await UserService.UnassignRoleAsync(
      roleId: roleId,
      userId: userId,
      orgId: orgId
    ));
  }

  public static async Task<IResult> DeleteUserAsync(string userId, string orgId)
  {
    return ApiResult.ToResult(await UserService.DeleteUserAsync(
      userId: userId,
      orgId: orgId
    ));
  }

  public static async Task<IResult> GetPropertiesAsync(string orgId, string userId, string? name)
  {
    return ApiResult.ToResult(await UserService.GetPropertiesAsync(userId: userId, name: name ?? Settings.Wildcard, orgId: orgId));
  }

  public static async Task<IResult> UpdatePropertyAsync(string orgId, string userId, string name, UpdateProperty update)
  {
    return ApiResult.ToResult(await UserService.UpdatePropertyAsync(orgId: orgId, userId: userId, name: name, value: update.Value, hidden: update.Hidden));
  }

  public static async Task<IResult> DeletePropertyAsync(string orgId, string userId, string name)
  {
    return ApiResult.ToResult(await UserService.DeletePropertyAsync(orgId: orgId, userId: userId, name: name));
  }
}