using System.Text;

namespace OpenApi2MeramaidConverter.Mermaid.ClassDiagram;

public class Node : IEqualityComparer<Node>
{
    public readonly string Id;
    public readonly string Description;
    public readonly List<NodeProperty> Properties;

    public Node(string id, IEnumerable<NodeProperty> properties = null)
    {
        Id = id;
        Properties = new List<NodeProperty>(properties);
    }
    
    public Node(string id, string description, IEnumerable<NodeProperty> properties = null)
    {
        Id = id;
        Description = StringExtensions.LineBreak(description, 30);
        Properties = new List<NodeProperty>(properties);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"class {Id}");

        if (string.IsNullOrEmpty(Description) == false)
        {
            sb.AppendLine($"        note for {Id} \"{Description}\"");
        }

        foreach (var property in Properties)
        {
            if (string.IsNullOrEmpty(property.SubType))
            {
                sb.AppendLine($"        {Id} : {property.Type} {property.Name}");
            }
            
            if (property.Type == "object" && string.IsNullOrEmpty(property.SubType) == false)
            {
                sb.AppendLine($"        {Id} : {property.SubType} {property.Name}");
            }
            
            if (property.Type == "array" && string.IsNullOrEmpty(property.SubType) == false)
            {
                sb.AppendLine($"        {Id} : {property.SubType}[] {property.Name}");
            }

        }

        return sb.ToString();
    }

    public bool Equals(Node x, Node y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Node obj)
    {
        return obj.Id.GetHashCode();
    }
}

public class NodeProperty
{
    public readonly string Name;
    public readonly string Type;
    public readonly string SubType;

    public NodeProperty(string name, string type, string subType = null)
    {
        Name = name;
        Type = type;
        SubType = subType;
    }
}