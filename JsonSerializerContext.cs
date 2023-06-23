using System.Text.Json.Serialization;
using tankman.Models;

namespace tankman.Serialization;

[JsonSerializable(typeof(Org[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
