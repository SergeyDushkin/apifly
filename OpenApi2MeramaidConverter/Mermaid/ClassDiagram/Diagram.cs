using System.Text;

namespace OpenApi2MeramaidConverter.Mermaid.ClassDiagram;

public class Diagram
{
    private readonly string Title;
    private Dictionary<string, Node> _nodes = new();
    private HashSet<Rel> _rels = new HashSet<Rel>();

    public Diagram(string title)
    {
        this.Title = title;
    }

    public void AddNode(Node node)
    {
        _nodes[node.Id] = node;
    }

    public void AddRel(Rel rel)
    {
        _rels.Add(rel);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        var settings = @"%%{
  init: {
    ""theme"": ""neutral"",
        ""fontFamily"": ""monospace"",
        ""logLevel"": ""info"",
        ""flowchart"": {
            ""htmlLabels"": true,
            ""curve"": ""linear""
        },
        ""sequence"": {
            ""mirrorActors"": true
        }
    }
}%%";
        sb.AppendLine(settings);
        sb.AppendLine("---");
        sb.AppendLine($"    title: {this.Title}");
        sb.AppendLine("---");
        sb.AppendLine("    classDiagram");

        foreach (var node in _nodes)
        {
            sb.AppendLine("    " + node.Value);
        }
        
        foreach (var rel in _rels)
        {
            sb.AppendLine("    " + rel);
        }
        
        return sb.ToString();
    }
}