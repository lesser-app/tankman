using tankman.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace tankman.Models;

public interface IEntity
{
  string Id { get; }
}

public interface IPathedEntity
{
  string Id { get; }
}

public interface IOrgAssociated
{
  string OrgId { get; }
}

public interface IUserAssociated
{
  string UserId { get; }
}

public interface IRoleAssociated
{
  string RoleId { get; }
}


public interface IActionAssociated
{
  string Action { get; }
}

public interface IResourceAssociated
{
  string ResourceId { get; }
}

public static class Filters
{
  public static IQueryable<T> ApplyIdFilter<T>(this IQueryable<T> baseQuery, string id) where T : IEntity
  {
    if (id == Settings.Wildcard)
    {
      return baseQuery;
    }
    else
    {
      var userList = id.Split(Settings.Separator).ToList();
      return userList.Count == 1 ? baseQuery.Where((x) => x.Id == id) : baseQuery.Where((x) => userList.Contains(x.Id));
    }
  }

  public static IQueryable<T> ApplyPathedIdFilter<T>(this IQueryable<T> baseQuery, string id) where T : IPathedEntity
  {
    return ApplyPathedFilterImpl(
      baseQuery,
      id,
      (normalizedPath) => (x) => x.Id == normalizedPath || EF.Functions.ILike(x.Id, $"{normalizedPath}/%"),
      (normalizedPath) => (x) => x.Id == normalizedPath
      );
  }

  public static IQueryable<T> ApplyOrgFilter<T>(this IQueryable<T> baseQuery, string orgId) where T : IOrgAssociated
  {
    return baseQuery.Where((x) => x.OrgId == orgId);
  }

  public static IQueryable<T> ApplyUsersFilter<T>(this IQueryable<T> baseQuery, string userId) where T : IUserAssociated
  {
    if (userId == Settings.Wildcard)
    {
      return baseQuery;
    }
    else
    {
      var userList = userId.Split(Settings.Separator).ToList();
      return userList.Count == 1 ? baseQuery.Where((x) => x.UserId == userId) : baseQuery.Where((x) => userList.Contains(x.UserId));
    }
  }

  public static IQueryable<T> ApplyRolesFilter<T>(this IQueryable<T> baseQuery, string roleId) where T : IRoleAssociated
  {
    if (roleId == Settings.Wildcard)
    {
      return baseQuery;
    }
    else
    {
      var roleList = roleId.Split(Settings.Separator).ToList();
      return roleList.Count == 1 ? baseQuery.Where((x) => x.RoleId == roleId) : baseQuery.Where((x) => roleList.Contains(x.RoleId));
    }
  }

  public static IQueryable<T> ApplyActionsFilter<T>(this IQueryable<T> baseQuery, string action) where T : IActionAssociated
  {
    if (action == Settings.Wildcard)
    {
      return baseQuery;
    }
    else
    {
      var actionList = action.Split(Settings.Separator).ToList();
      return actionList.Count == 1 ? baseQuery.Where((x) => x.Action == action) : baseQuery.Where((x) => actionList.Contains(x.Action));
    }
  }

  public static IQueryable<T> ApplyResourceFilter<T>(this IQueryable<T> baseQuery, string resourceId) where T : IResourceAssociated
  {
    return ApplyPathedFilterImpl(
      baseQuery,
      resourceId,
      (normalizedPath) => (x) => x.ResourceId == normalizedPath || EF.Functions.ILike(x.ResourceId, $"{normalizedPath}/%"),
      (normalizedPath) => (x) => x.ResourceId == normalizedPath);
  }

  private static IQueryable<T> ApplyPathedFilterImpl<T>(this IQueryable<T> baseQuery, string id, Func<string, Expression<Func<T, bool>>> makeWildcardPredicate, Func<string, Expression<Func<T, bool>>> makeExactPredicate)
  {
    var isWildCardResource = id.EndsWith(Settings.Wildcard);

    if (isWildCardResource)
    {
      id = Paths.StripFromEnd(id, Settings.Wildcard);
    }

    var normalizedPath = Paths.Normalize(id);

    var wildcardPredicate = makeWildcardPredicate(normalizedPath);
    var exactPredicate = makeExactPredicate(normalizedPath);

    return isWildCardResource
      ? (normalizedPath == "")
        ? baseQuery
        : baseQuery.Where(wildcardPredicate)
      : baseQuery.Where(exactPredicate);
  }

}