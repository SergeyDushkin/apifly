var path = args.Length > 0 ? args[0] : Environment.CurrentDirectory;
var pattern = "*.yaml";
var searchOption = SearchOption.AllDirectories;

if (File.Exists(path))
{
    path = Path.GetPathRoot(path);
    pattern = Path.GetFileName(path);
    searchOption = SearchOption.TopDirectoryOnly;
}

var files = Directory
    .GetFiles(path, pattern, searchOption)
    .Where(f => File.ReadAllLines(f).Any(l => l.Contains("openapi:", StringComparison.InvariantCultureIgnoreCase)))
    .ToList();

foreach (var filepath in files)
{
    var diagram = Mermaid.OpenAPI2Mermaid(filepath);
    var fileName = Path.GetFileNameWithoutExtension(filepath) + ".mmd";
    File.WriteAllText(fileName, diagram.ToString());
}