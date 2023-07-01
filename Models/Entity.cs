namespace tankman.Models;

public interface IEntity
{
  string Id { get; }
}

public interface IPathSearchHelperEntityInJoin<T> where T : IPathSearchHelperEntity
{
  T ResourcePath { get; }
}

public interface IDynamicProperty
{
  string Name { get; }
  string Value { get; }
  bool Hidden { get; }
}

public interface IHasDynamicProperties<T> where T : IDynamicProperty
{
  List<T> Properties { get; }
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

public static class DynamicPropertyExtensions
{
  public static Dictionary<string, string> ToDictionary<T>(this List<T> properties) where T : IDynamicProperty
  {
    var results = new Dictionary<string, string>();

    foreach (var prop in properties)
    {
      results[prop.Name] = prop.Value;
    }

    return results;
  }
}