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

public interface IPathScanEntityInJoin<T> where T : IPathScanEntity
{
  T ResourcePath { get; }
}

public interface IPathScanEntity
{
  string ResourceId { get; }
  string ParentId { get; }
  string Root1Id { get; }
  string Root2Id { get; }
  string Root3Id { get; }
  string Root4Id { get; }
  string Root5Id { get; }
  string Root6Id { get; }
  string Root7Id { get; }
  string Root8Id { get; }
  string Root9Id { get; }
  string Root10Id { get; }
  string Root11Id { get; }
  string Root12Id { get; }
  string Root13Id { get; }
  string Root14Id { get; }
  string Root15Id { get; }
  string Root16Id { get; }
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

  public static IQueryable<T> ApplyPathedExactIdFilter<T>(this IQueryable<T> baseQuery, string id) where T : IPathedEntity
  {
    return baseQuery.Where((x) => x.Id == id);
  }

  public static IQueryable<TJoin> ApplyPathScanFilterToJoin<TJoin, TPathScanEntity>(this IQueryable<TJoin> baseQuery, string normalizedResourceId, int partsLength)
    where TJoin : IPathScanEntityInJoin<TPathScanEntity>
    where TPathScanEntity : IPathScanEntity
  {
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    return (partsLength == 1
                ? baseQuery.Where((x) => x.ResourcePath.Root1Id == normalizedResourceId)
                : partsLength == 2
                ? baseQuery.Where((x) => x.ResourcePath.Root2Id == normalizedResourceId)
                : partsLength == 3
                ? baseQuery.Where((x) => x.ResourcePath.Root3Id == normalizedResourceId)
                : partsLength == 4
                ? baseQuery.Where((x) => x.ResourcePath.Root4Id == normalizedResourceId)
                : partsLength == 5
                ? baseQuery.Where((x) => x.ResourcePath.Root5Id == normalizedResourceId)
                : partsLength == 6
                ? baseQuery.Where((x) => x.ResourcePath.Root6Id == normalizedResourceId)
                : partsLength == 7
                ? baseQuery.Where((x) => x.ResourcePath.Root7Id == normalizedResourceId)
                : partsLength == 8
                ? baseQuery.Where((x) => x.ResourcePath.Root8Id == normalizedResourceId)
                : throw new Exception("Internal error. Shouldn't get here."));
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

  public static IQueryable<T> ApplyExactResourceFilter<T>(this IQueryable<T> baseQuery, string resourceId) where T : IResourceAssociated
  {
    return baseQuery.Where((x) => x.ResourceId == resourceId);
  }


  // public static IQueryable<T> ApplyResourceFilter<T>(this IQueryable<T> baseQuery, string resourceId) where T : IResourceAssociated
  // {
  //   return ApplyPathedFilterImpl(
  //     baseQuery,
  //     resourceId,
  //     (normalizedPath) => (x) => x.ResourceId == normalizedPath || EF.Functions.ILike(x.ResourceId, $"{normalizedPath}/%"),
  //     (normalizedPath) => (x) => x.ResourceId == normalizedPath);
  // }

  // private static IQueryable<T> ApplyPathedFilterImpl<T>(this IQueryable<T> baseQuery, string id, Func<string, Expression<Func<T, bool>>> makeWildcardPredicate, Func<string, Expression<Func<T, bool>>> makeExactPredicate)
  // {
  //   var isWildCardResource = id.EndsWith(Settings.Wildcard);

  //   if (isWildCardResource)
  //   {
  //     id = Paths.StripFromEnd(id, Settings.Wildcard);
  //   }

  //   var normalizedPath = Paths.Normalize(id);

  //   var wildcardPredicate = makeWildcardPredicate(normalizedPath);
  //   var exactPredicate = makeExactPredicate(normalizedPath);

  //   return isWildCardResource
  //     ? (normalizedPath == "")
  //       ? baseQuery
  //       : baseQuery.Where(wildcardPredicate)
  //     : baseQuery.Where(exactPredicate);
  // }

}