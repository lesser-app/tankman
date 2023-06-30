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

public interface IPathSearchHelperEntityInJoin<T> where T : IPathSearchHelperEntity
{
  T ResourcePath { get; }
}

public interface IPathSearchHelperEntity
{
  string ResourceId { get; }
  string ParentId { get; }
  string Parent1Id { get; }
  string Parent2Id { get; }
  string Parent3Id { get; }
  string Parent4Id { get; }
  string Parent5Id { get; }
  string Parent6Id { get; }
  string Parent7Id { get; }
  string Parent8Id { get; }
  string Parent9Id { get; }
  string Parent10Id { get; }
  string Parent11Id { get; }
  string Parent12Id { get; }
  string Parent13Id { get; }
  string Parent14Id { get; }
  string Parent15Id { get; }
  string Parent16Id { get; }
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

  public static IQueryable<TJoin> UsePathScanOptimization<TJoin, TPathScanEntity>(this IQueryable<TJoin> baseQuery, string normalizedResourceId, int partsLength)
    where TJoin : IPathSearchHelperEntityInJoin<TPathScanEntity>
    where TPathScanEntity : IPathSearchHelperEntity
  {
    return (partsLength == 1
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent1Id == normalizedResourceId)
                : partsLength == 2
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent2Id == normalizedResourceId)
                : partsLength == 3
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent3Id == normalizedResourceId)
                : partsLength == 4
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent4Id == normalizedResourceId)
                : partsLength == 5
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent5Id == normalizedResourceId)
                : partsLength == 6
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent6Id == normalizedResourceId)
                : partsLength == 7
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent7Id == normalizedResourceId)
                : partsLength == 8
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent8Id == normalizedResourceId)
                : partsLength == 9
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent9Id == normalizedResourceId)
                : partsLength == 10
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent10Id == normalizedResourceId)
                : partsLength == 11
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent11Id == normalizedResourceId)
                : partsLength == 12
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent12Id == normalizedResourceId)
                : partsLength == 13
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent13Id == normalizedResourceId)
                : partsLength == 14
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent14Id == normalizedResourceId)
                : partsLength == 15
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent15Id == normalizedResourceId)
                : partsLength == 16
                ? baseQuery.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent16Id == normalizedResourceId)
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

  public static IQueryable<T> ApplyExactResourceFilter<T>(this IQueryable<T> baseQuery, string normalizedResourceId) where T : IResourceAssociated
  {
    return baseQuery.Where((x) => x.ResourceId == normalizedResourceId);
  }
}