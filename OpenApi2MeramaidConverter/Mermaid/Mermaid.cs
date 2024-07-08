using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using OpenApi2MeramaidConverter;
using OpenApi2MeramaidConverter.Mermaid.ClassDiagram;

public static class Mermaid
{
    public static Diagram OpenAPI2Mermaid(string filepath)
    {
        Func<KeyValuePair<string, OpenApiSchema>, string> getSubType = (node =>
        {
            switch (node.Value.Type)
            {
                case "object":
                    return node.Value.Reference?.Id;
                case "array":
                    return node.Value.Items.Reference?.Id;
            }

            return string.Empty;
        });
        
        var referenceList = new HashSet<string>();
        
        using (var reader = new StreamReader(filepath))
        {
            // Read V3 as YAML
            var openApiDocument = new OpenApiStreamReader().Read(reader.BaseStream, out var diagnostic);
    
            var diagram = new Diagram(openApiDocument.Info.Title);

            foreach (var path in openApiDocument.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    foreach (var response in operation.Value.Responses)
                    {
                        foreach (var content in response.Value.Content)
                        {
                            referenceList.Add(content.Value.Schema.Reference.Id);
                        }
                    }
                }
            }
    
            foreach (var component in openApiDocument.Components.Schemas.Where(r => referenceList.Contains(r.Key)))
            {
                var relations = OpenApiExtensions.FlatRef(component.Key, null, RelTypes.Link, openApiDocument.Components.Schemas)
                    .Distinct()
                    .ToList();

                var nodes = relations.Select(r => r.Left).Distinct();
                foreach (var node in nodes)
                {
                    var description = openApiDocument.Components.Schemas[node].Description;
                    var properies = openApiDocument.Components.Schemas[node].Properties
                        .Select(r => new NodeProperty(r.Key, r.Value.Type, getSubType(r)))
                        .ToList();
            
                    diagram.AddNode(new Node(node, description, properies));
                }

                foreach (var relation in relations.Where(r => string.IsNullOrEmpty(r.Right) == false))
                {
                    diagram.AddRel(relation);
                }
            }

            return diagram;
        }
    }
}