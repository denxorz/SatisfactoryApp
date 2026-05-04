using System.Text.Json;
using Denxorz.Satisfactory.Routes;

if (args.Length == 0 || HasHelpFlag(args))
{
    PrintUsage();
    return args.Length == 0 ? 1 : 0;
}

var savePath = args[0];
string? jsonOutPath = null;
var printJson = false;

for (var i = 1; i < args.Length; i++)
{
    var arg = args[i];
    switch (arg)
    {
        case "--json":
            printJson = true;
            break;
        case "--json-out":
            if (i + 1 >= args.Length)
            {
                Console.Error.WriteLine("Missing value for --json-out.");
                return 1;
            }

            jsonOutPath = args[++i];
            break;
        default:
            Console.Error.WriteLine($"Unknown argument: {arg}");
            PrintUsage();
            return 1;
    }
}

if (!File.Exists(savePath))
{
    Console.Error.WriteLine($"File not found: {savePath}");
    return 1;
}

try
{
    using var stream = File.OpenRead(savePath);
    var saveDetails = SaveDetails.LoadFromStream(stream);

    Console.WriteLine("Parse succeeded.");
    Console.WriteLine($"File: {savePath}");
    Console.WriteLine($"Factories: {saveDetails.Factories.Count}");
    Console.WriteLine($"PowerCircuits: {saveDetails.PowerCircuits.Count}");
    Console.WriteLine($"Stations: {saveDetails.Stations.Count}");
    Console.WriteLine($"Uploaders: {saveDetails.Uploaders.Count}");
    Console.WriteLine($"Resources: {saveDetails.Resources.Count}");

    if (printJson || !string.IsNullOrWhiteSpace(jsonOutPath))
    {
        var json = JsonSerializer.Serialize(saveDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        if (printJson)
        {
            Console.WriteLine(json);
        }

        if (!string.IsNullOrWhiteSpace(jsonOutPath))
        {
            var outputDirectory = Path.GetDirectoryName(Path.GetFullPath(jsonOutPath));
            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            File.WriteAllText(jsonOutPath, json);
            Console.WriteLine($"Wrote JSON to: {jsonOutPath}");
        }
    }

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine("Parse failed.");
    Console.Error.WriteLine($"File: {savePath}");
    WriteExceptionChain(ex);
    return 2;
}

static bool HasHelpFlag(IEnumerable<string> values)
    => values.Any(v => v is "-h" or "--help");

static void PrintUsage()
{
    Console.WriteLine("""
Usage:
  dotnet run --project scripts/SaveProbe -- "<path-to-save.sav>" [--json] [--json-out <path>]

Options:
  --json               Print parsed JSON to stdout.
  --json-out <path>    Write parsed JSON to a file.
  -h, --help           Show help.
""");
}

static void WriteExceptionChain(Exception ex)
{
    var index = 0;
    Exception? current = ex;
    while (current is not null)
    {
        var label = index == 0 ? "Error" : $"Inner[{index}]";
        Console.Error.WriteLine($"{label}: {current.GetType().FullName}");
        Console.Error.WriteLine($"{label} message: {current.Message}");
        current = current.InnerException;
        index++;
    }

    Console.Error.WriteLine("Stack trace:");
    Console.Error.WriteLine(ex.StackTrace);
}
