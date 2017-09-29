using CitizenFX.Core;
using System;
using System.IO;

internal static class Log
{
    static object _lock = new object();
    static StreamWriter writer;

    public static void Create(string fileName)
    {
        writer = new StreamWriter(fileName);
    }

    public static void WriteLine(string line)
    {
        lock (_lock)
        {
            string formatted = $"[{DateTime.Now.ToString()}]: {line}";
            writer.WriteLine(formatted);
            writer.Flush();

            Debug.WriteLine($"(DispatchSystem) {formatted}");
        }
    }
    public static void WriteLineSilent(string line)
    {
        lock (_lock)
        {
            string formatted = $"[{DateTime.Now.ToString()}]: {line}";
            writer.WriteLine(formatted);
            writer.Flush();
        }
    }
}