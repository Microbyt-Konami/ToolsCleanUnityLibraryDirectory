
var pathsPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.FullName)!, "Directories.txt");
var paths = await File.ReadAllLinesAsync(pathsPath);

Console.WriteLine("Directories:");
foreach (var path in paths)
{
    Console.WriteLine($"- {path}");
}

Console.Write("Correct (Y/N)? ");

if (!string.Equals(Console.ReadLine(), "Y", StringComparison.InvariantCultureIgnoreCase))
{
    Console.WriteLine("Aborting.");

    return;
}

ShowReadDirectoriesMessage();

var stack = new Stack<(string[] dirs, int pos)>();

stack.Push((paths, 0));

while (stack.Count > 0)
{
    var (dirs, pos) = stack.Pop();

    if (pos >= dirs.Length)
        continue;

    stack.Push((dirs, pos + 1));

    var dir = dirs[pos];

    DirectoryInfo[]? dir2s;

    try
    {
        dir2s = new DirectoryInfo(dir).GetDirectories();
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"Error reading {dir}: {ex.Message}");
        ShowReadDirectoriesMessage();
        dir2s = [];
    }

    var dirNames = dir2s.Select(d => d.Name).ToArray();

    if (dirNames.Contains("Library") && File.Exists($"{dir}\\{Path.GetFileName(dir)}.sln"))
    {
        Console.WriteLine();
        Console.Write($"Delete {dir}/Library (Y/N)? ");
        if (string.Equals(Console.ReadLine(), "Y", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.WriteLine("Deleting ...");
            Directory.Delete($"{dir}/Library", true);
            Console.WriteLine("Done.");
        }
        ShowReadDirectoriesMessage();
    }
    else if (dir2s.Length > 0)
        stack.Push((dir2s.Select(d => d.FullName).ToArray(), 0));
}

Console.WriteLine("Done.");

void ShowReadDirectoriesMessage()
{
    Console.Write("Read directories ...");
}