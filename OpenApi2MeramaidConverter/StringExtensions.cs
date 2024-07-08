namespace OpenApi2MeramaidConverter;

public static class StringExtensions
{
    public static IEnumerable<string> SplitToLines(string stringToSplit, int maximumLineLength)
    {
        var words = stringToSplit.Split(' ');
        var line = words.First();
        foreach (var word in words.Skip(1))
        {
            var test = $"{line} {word}";
            if (test.Length > maximumLineLength)
            {
                yield return line;
                line = word;
            }
            else
            {
                line = test;
            }
        }
        yield return line;
    }
    
    public static string LineBreak(this string stringToSplit, int maximumLineLength)
    {
        var lines = SplitToLines(stringToSplit, maximumLineLength);
        return string.Join(Environment.NewLine, lines);
    }
}