using tankman.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace tankman.Models;

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
      if (userId.Contains(Settings.Separator))
      {
        var userList = userId.Split(Settings.Separator).ToList();
        return baseQuery.Where((x) => userList.Contains(x.UserId));
      }
      else
      {
        return baseQuery.Where((x) => x.UserId == userId);
      }
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
      if (roleId.Contains(Settings.Separator))
      {
        var roleList = roleId.Split(Settings.Separator).ToList();
        return baseQuery.Where((x) => roleList.Contains(x.RoleId));
      }
      else
      {
        return baseQuery.Where((x) => x.RoleId == roleId);
      }
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

  public static IQueryable<T> ApplySkip<T>(this IQueryable<T> baseQuery, int? from)
  {
    return from.HasValue ? baseQuery.Skip(from.Value) : baseQuery;
  }

  public static IQueryable<T> ApplyLimit<T>(this IQueryable<T> baseQuery, int? limit = null)
  {
    return limit.HasValue ? baseQuery.Take(limit.Value) : baseQuery.Take(Settings.MaxResults);
  }

  public static IQueryable<T> ApplyExactResourceFilter<T>(this IQueryable<T> baseQuery, string normalizedResourceId) where T : IResourceAssociated
  {
    return baseQuery.Where((x) => x.ResourceId == normalizedResourceId);
  }

  public static IQueryable<T> ApplyPropertiesFilter<T, TProp>(this IQueryable<T> baseQuery, Dictionary<string, string> matches) where T : IHasDynamicProperties<TProp> where TProp : IDynamicProperty
  {
    foreach (var item in matches)
    {
      baseQuery = baseQuery.Where(x => x.Properties.Any(p => p.Name == item.Key && p.Value == item.Value));
    }
    return baseQuery;
  }

  public static IQueryable<T> SelectProperties<T>(this IQueryable<T> baseQuery, string name) where T : IDynamicProperty
  {
    if (name == Settings.Wildcard)
    {
      return baseQuery.Where(x => !x.Hidden);
    }
    else if (name.Contains(Settings.Separator))
    {
      var names = name.Split(Settings.Separator);
      return baseQuery.Where(x => names.Contains(x.Name));
    }
    else
    {
      return baseQuery.Where(x => x.Name == name);
    }
  }
}