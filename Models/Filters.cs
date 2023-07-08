using tankman.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace tankman.Models;

public static class Filters
{
  public static IQueryable<T> FilterById<T>(this IQueryable<T> query, string id) where T : IEntity
  {
    return query.Where((x) => x.Id == id);
  }

  public static IQueryable<T> FilterByIdPattern<T>(this IQueryable<T> query, string id) where T : IEntity
  {
    if (id == Settings.Wildcard)
    {
      return query;
    }
    else
    {
      var userList = id.Split(Settings.Separator).ToList();
      return userList.Count == 1 ? query.Where((x) => x.Id == id) : query.Where((x) => userList.Contains(x.Id));
    }
  }

  public static IQueryable<TJoin> FilterWithPathScanOptimization<TJoin, TPathScanEntity>(this IQueryable<TJoin> query, string normalizedResourceId, int partsLength)
    where TJoin : IPathSearchHelperEntityInJoin<TPathScanEntity>
    where TPathScanEntity : IPathSearchHelperEntity
  {
    return (partsLength == 1
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent1Id == normalizedResourceId)
                : partsLength == 2
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent2Id == normalizedResourceId)
                : partsLength == 3
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent3Id == normalizedResourceId)
                : partsLength == 4
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent4Id == normalizedResourceId)
                : partsLength == 5
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent5Id == normalizedResourceId)
                : partsLength == 6
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent6Id == normalizedResourceId)
                : partsLength == 7
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent7Id == normalizedResourceId)
                : partsLength == 8
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent8Id == normalizedResourceId)
                : partsLength == 9
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent9Id == normalizedResourceId)
                : partsLength == 10
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent10Id == normalizedResourceId)
                : partsLength == 11
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent11Id == normalizedResourceId)
                : partsLength == 12
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent12Id == normalizedResourceId)
                : partsLength == 13
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent13Id == normalizedResourceId)
                : partsLength == 14
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent14Id == normalizedResourceId)
                : partsLength == 15
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent15Id == normalizedResourceId)
                : partsLength == 16
                ? query.Where((x) => x.ResourcePath.ResourceId == normalizedResourceId || x.ResourcePath.Parent16Id == normalizedResourceId)
                : throw new Exception("Internal error. Shouldn't get here."));
  }

  public static IQueryable<T> FilterByOrg<T>(this IQueryable<T> query, string orgId) where T : IOrgAssociated
  {
    if (orgId == Settings.Wildcard)
    {
      return query;
    }
    else
    {
      var orgsList = orgId.Split(Settings.Separator).ToList();
      return orgsList.Count == 1 ? query.Where((x) => x.OrgId == orgId) : query.Where((x) => orgsList.Contains(x.OrgId));
    }
  }

  public static IQueryable<T> FilterByUser<T>(this IQueryable<T> query, string userId) where T : IUserAssociated
  {
    return query.Where((x) => x.UserId == userId);
  }

  public static IQueryable<T> FilterByUserPattern<T>(this IQueryable<T> query, string userId) where T : IUserAssociated
  {
    if (userId == Settings.Wildcard)
    {
      return query;
    }
    else
    {
      if (userId.Contains(Settings.Separator))
      {
        var userList = userId.Split(Settings.Separator).ToList();
        return query.Where((x) => userList.Contains(x.UserId));
      }
      else
      {
        return query.Where((x) => x.UserId == userId);
      }
    }
  }

  public static IQueryable<T> FilterByRole<T>(this IQueryable<T> query, string roleId) where T : IRoleAssociated
  {
    return query.Where((x) => x.RoleId == roleId);
  }

  public static IQueryable<T> FilterByRolePattern<T>(this IQueryable<T> query, string roleId) where T : IRoleAssociated
  {
    if (roleId == Settings.Wildcard)
    {
      return query;
    }
    else
    {
      if (roleId.Contains(Settings.Separator))
      {
        var roleList = roleId.Split(Settings.Separator).ToList();
        return query.Where((x) => roleList.Contains(x.RoleId));
      }
      else
      {
        return query.Where((x) => x.RoleId == roleId);
      }
    }
  }

  public static IQueryable<T> FilterByActionPattern<T>(this IQueryable<T> query, string action) where T : IActionAssociated
  {
    if (action == Settings.Wildcard)
    {
      return query;
    }
    else
    {
      var actionList = action.Split(Settings.Separator).ToList();
      return actionList.Count == 1 ? query.Where((x) => x.Action == action) : query.Where((x) => actionList.Contains(x.Action));
    }
  }

  public static IQueryable<T> ApplySkip<T>(this IQueryable<T> query, int? from)
  {
    return from.HasValue ? query.Skip(from.Value) : query;
  }

  public static IQueryable<T> ApplyLimit<T>(this IQueryable<T> query, int? limit = null)
  {
    return limit.HasValue ? query.Take(limit.Value) : query.Take(Settings.MaxResults);
  }

  public static IQueryable<T> FilterByResource<T>(this IQueryable<T> query, string normalizedResourceId) where T : IResourceAssociated
  {
    return query.Where((x) => x.ResourceId == normalizedResourceId);
  }

  public static IQueryable<T> FilterByProperties<T, TProp>(this IQueryable<T> query, Dictionary<string, string> matches) where T : IHasDynamicProperties<TProp> where TProp : DynamicProperty
  {
    foreach (var item in matches)
    {
      query = query.Where(x => x.Properties.Any(p => p.Name == item.Key && p.Value == item.Value));
    }
    return query;
  }

  public static IQueryable<T> SelectProperties<T>(this IQueryable<T> query, string name) where T : DynamicProperty
  {
    if (name == Settings.Wildcard)
    {
      return query.Where(x => !x.Hidden);
    }
    else if (name.Contains(Settings.Separator))
    {
      var names = name.Split(Settings.Separator);
      return query.Where(x => names.Contains(x.Name));
    }
    else
    {
      return query.Where(x => x.Name == name);
    }
  }
}