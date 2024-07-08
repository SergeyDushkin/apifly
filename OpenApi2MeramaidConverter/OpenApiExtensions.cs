using Microsoft.OpenApi.Models;
using OpenApi2MeramaidConverter.Mermaid.ClassDiagram;

namespace OpenApi2MeramaidConverter;

public static class OpenApiExtensions
{
    public static IEnumerable<Rel> FlatRef(string id, string parentId, RelTypes relation, IDictionary<string,OpenApiSchema> schema)
    {
        var node = schema[id];
        var refer = node.Properties
            .Where(r => new [] { "object", "array" }.Contains(r.Value.Type) 
                        || r.Value.OneOf.Any()
                        || r.Value.AllOf.Any()
                        || r.Value.AnyOf.Any())
            .ToList();
        
        yield return new Rel(id, parentId, relation);
        
        if (node.AllOf.Any())
        {
            foreach (var v in node.AllOf)
            {
                if (v.Reference == null) continue;
                
                foreach (var item in FlatRef(v.Reference.Id, id, RelTypes.Inheritance, schema))
                {
                    yield return item;
                }
            }
        }
        
        foreach (var r in refer)
        {
            if (r.Value.Type == "array"&& r.Value.Items.Reference != null)
            {
                foreach (var item in FlatRef(r.Value.Items.Reference.Id, id, RelTypes.Aggregation, schema))
                {
                    yield return item;
                }     
            }
            
            if (r.Value.Type == "object" && r.Value.Reference != null)
            {
                foreach (var item in FlatRef(r.Value.Reference.Id, id, RelTypes.Aggregation, schema))
                {
                    yield return item;
                }     
            }

            if (r.Value.OneOf.Any())
            {
                foreach (var v in r.Value.OneOf)
                {
                    foreach (var item in FlatRef(v.Reference.Id, id, RelTypes.Association, schema))
                    {
                        yield return item;
                    }
                }
            }
            
            if (r.Value.AllOf.Any())
            {
                foreach (var v in r.Value.AllOf)
                {
                    foreach (var item in FlatRef(v.Reference.Id, id, RelTypes.Association, schema))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}