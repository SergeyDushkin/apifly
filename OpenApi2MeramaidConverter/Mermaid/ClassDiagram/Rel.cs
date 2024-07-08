namespace OpenApi2MeramaidConverter.Mermaid.ClassDiagram;

public enum RelTypes
{
    Inheritance,
    Composition,
    Aggregation,
    Association,
    Link,
    Dependency,
    Realization
}

public class Rel : IEquatable<Rel>
{
    public readonly string Left;
    public readonly string Right;
    public readonly string Label;
    public readonly RelTypes Relation;

    public Rel(string left, string right, RelTypes relation, string label = null)
    {
        Left = left;
        Right = right;
        Relation = relation;
        Label = label;
    }

    public override string ToString()
    {
        var relationSign = Relation switch
        {
            RelTypes.Aggregation => "--o",
            RelTypes.Association => "-->",
            RelTypes.Composition => "--*",
            RelTypes.Link => "--",
            RelTypes.Dependency => "..>",
            RelTypes.Inheritance => "<|--",
            RelTypes.Realization => "..|>",
        };

        return $"{Left} {relationSign} {Right}" + (string.IsNullOrEmpty(Label) ? String.Empty : $" : {Label}");
    }
    
    public bool Equals(Rel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Left == other.Left && Right == other.Right && Label == other.Label && Relation == other.Relation;
    }
    
    public override int GetHashCode()
    {
        return Left.GetHashCode() ^
               ((string.IsNullOrEmpty(Right) != true) ? Right.GetHashCode() : string.Empty.GetHashCode()) ^
               ((string.IsNullOrEmpty(Label) != true) ? Label.GetHashCode() : string.Empty.GetHashCode()) ^
               Relation.GetHashCode();
    }
}