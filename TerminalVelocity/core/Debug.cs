using System;
using System.Collections.Generic;
using System.IO;

namespace TerminalVelocity.core;
public class Debug
{
    private static readonly Debug instance = new Debug();
    private static readonly object lockObject = new object(); // For thread safety
    private static readonly LinkedList<string> debugEntries = new LinkedList<string>(); // Maintains insertion order
    private static int longestPrefix = 0;
    private static int maxLogSize = 1000;
    private static LogLevel currentLogLevel = LogLevel.Game; // Default to logging game logs

    private Debug() { }

    public static Debug Instance => instance;

    // Enum for logging levels
    public enum LogLevel
    {
        None,
        Game,
        Error,
        Important,
        Everything // Log all levels
    }

    // Property to set/get the maximum log size
    public static int MaxLogSize
    {
        get => maxLogSize;
        set
        {
            if (value > 0)
            {
                lock (lockObject)
                {
                    maxLogSize = value;
                    TrimLogSize(); // Trim entries if needed
                }
            }
        }
    }

    // Property to get/set the current logging level
    public static LogLevel CurrentLogLevel
    {
        get => currentLogLevel;
        set
        {
            if (Enum.IsDefined(typeof(LogLevel), value))
            {
                currentLogLevel = value;
            }
        }
    }

    // Function to ensure log size stays within the maximum limit
    private static void TrimLogSize()
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
    private static void AddEntry(string _entry, object sender, LogLevel requiredLevel, string suffix = "")
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
    private static string CreatePrefix(object sender, string suffix = "")
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

    // Functions to add specific types of log entries
    public static void Log(string _entry, object sender) => AddEntry(_entry, sender, LogLevel.Game, "App");
    public static void AddDebugEntry(string _entry, object sender) => AddEntry(_entry, sender, LogLevel.Everything, "   ");

    public static void AddImportantEntry(string _entry, object sender) => AddEntry(_entry, sender, LogLevel.Important, " ! ");

    public static void AddErrorEntry(string _entry, object sender) => AddEntry(_entry, sender, LogLevel.Error, "!!!");

    // Function to print the current log to a file
    public static void PrintToFile(string filename)
    {
        var path = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(path, filename);

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
}
