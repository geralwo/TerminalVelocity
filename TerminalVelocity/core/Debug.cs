namespace TerminalVelocity.core;
using TerminalVelocity;
public class Debug
{
    private static readonly Debug instance = new Debug("Engine.log");
    public static Debug Instance => instance;

    private readonly object lockObject = new object(); // For thread safety
    private readonly LinkedList<string> debugEntries = new LinkedList<string>(); // Maintains insertion order
    private int longestPrefix = 0;
    private string FileName;
    public int maxLogSize = 1000;
    public LogLevel currentLogLevel = LogLevel.Game; // Default to logging game logs


    private Debug(string _filename)
    {
        FileName = _filename;
    }
    public Debug(LogLevel _logLevel, string _filename)
    {
        currentLogLevel = _logLevel;
        FileName = _filename;
    }

    // Property to set/get the maximum log size
    public static int MaxLogSize
    {
        get => Instance.maxLogSize;
        set
        {
            if (value > 0)
            {
                lock (Instance.lockObject)
                {
                    Instance.maxLogSize = value;
                    Instance.TrimLogSize(); // Trim entries if needed
                }
            }
        }
    }

    // Property to get/set the current logging level
    public static LogLevel CurrentLogLevel
    {
        get => Instance.currentLogLevel;
        set
        {
            if (Enum.IsDefined(typeof(LogLevel), value))
            {
                Instance.currentLogLevel = value;
            }
        }
    }

    // Function to ensure log size stays within the maximum limit
    private void TrimLogSize()
    {
        lock (lockObject) // Ensure thread safety
        {
            // While the log has more entries than allowed, remove the oldest ones
            while (debugEntries.Count >= maxLogSize)
            {
                debugEntries.RemoveFirst(); // Remove the oldest entry
            }
        }
    }

    // Function to add an entry to the log
    private void AddEntry(string _entry, object sender, LogLevel requiredLevel, string suffix = "")
    {
        if (currentLogLevel < requiredLevel) return; // Early exit if the logging level doesn't permit this entry

        var prefix = CreatePrefix(sender, suffix);
        var entry = $"{DateTime.Now.ToLongTimeString()}: {prefix}{_entry}";

        lock (lockObject) // Ensure thread safety
        {
            TrimLogSize(); // Ensure the log doesn't exceed the maximum size

            debugEntries.AddLast(entry); // Add the entry to the end of the list
        }
    }

    // Function to create a debug entry with a uniform prefix
    private string CreatePrefix(object sender, string suffix = "")
    {
        var className = sender.GetType().Name;
        var prefix = $"[{className}]";

        lock (lockObject) // Ensure thread safety
        {
            if (prefix.Length > longestPrefix)
                longestPrefix = prefix.Length;
        }

        return prefix.PadLeft(longestPrefix) + " " + suffix + " ";
    }

    // Function to print the current log to a file
    public void printToFile()
    {
        var path = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(path, FileName);

        try
        {
            using (var sw = new StreamWriter(filePath)) // File.WriteText also works
            {
                lock (lockObject) // Thread safety
                {
                    foreach (var entry in debugEntries)
                    {
                        sw.WriteLine(entry); // Write each log entry
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }

    public static void Log(string _entry, object sender) => Instance.AddEntry(_entry, sender, LogLevel.Game, "App");
    public static void AddPhysicsEntry(string _entry, object sender) => Instance.AddEntry(_entry, sender, LogLevel.Physics, "->|");
    public static void AddDebugEntry(string _entry, object sender) => Instance.AddEntry(_entry, sender, LogLevel.Everything, "   ");
    public static void AddImportantEntry(string _entry, object sender) => Instance.AddEntry(_entry, sender, LogLevel.Important, " ! ");
    public static void AddErrorEntry(string _entry, object sender) => Instance.AddEntry(_entry, sender, LogLevel.Error, "!!!");
    public static void PrintToFile() => Instance.printToFile();
}
