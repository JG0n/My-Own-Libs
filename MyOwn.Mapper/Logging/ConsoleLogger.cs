using System;

class ConsoleLogger : ILogger
{
    public void Warning(string text) => Log(text, ConsoleColor.Yellow);

    public void Error(string text) => Log(text, ConsoleColor.Red);

    public void Info(string text) => Log(text, ConsoleColor.Green);

    public void Log(string text, ConsoleColor color)
    {
        ConsoleColor currentColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Log(text);
        Console.ForegroundColor = currentColor;
    }

    public void Log(string text) => Console.WriteLine($"LOG - {text}");
}