using CitizenFX.Core;
using System;
using System.Globalization;
using System.IO;

internal static class Log
{
    static readonly object _lock = new object(); // lock required for no double-logging
    private static StreamWriter writer; // writer for writing the log to lines in the file

    public static void Create(string fileName)
    {
        lock (_lock)
        {
            // creating the writer object for the endpoint
            writer = new StreamWriter(fileName);
        }
    }

    /// <summary>
    /// Writes a log to the server console and the file
    /// </summary>
    /// <param name="line"></param>
    public static void WriteLine(string line)
    {
        lock (_lock)
        {
            // creating the string formatted with date
            string formatted = $"[{DateTime.Now.ToString("HH:mm:ss.fff")}]: {line}";
            // writing the formatted line to the log
            writer.WriteLine(formatted);
            writer.Flush();

            Debug.WriteLine($"(DispatchSystem) {formatted}"); // writing log to server console too
        }
    }
    /// <summary>
    /// Writes a log just to the file
    /// </summary>
    /// <param name="line"></param>
    public static void WriteLineSilent(string line)
    {
        lock (_lock)
        {
            // creating the string formatted with date
            string formatted = $"[{DateTime.Now.ToString("HH:mm:ss.fff")}]: {line}";
            // writing the formatted line to the log
            writer.WriteLine(formatted);
            writer.Flush();
        }
    }
}