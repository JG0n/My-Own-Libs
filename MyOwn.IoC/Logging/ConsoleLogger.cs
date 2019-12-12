using System;

class ConsoleLogger : ILogger
{
    public void Warning(string text)
    {
        System.ConsoleColor currentColor = System.Console.ForegroundColor;

        System.Console.ForegroundColor = System.ConsoleColor.Yellow;
        System.Console.WriteLine($"LOG - {text}");
        System.Console.ForegroundColor = currentColor;
    }

    public void Log(string text)
    {
        System.Console.WriteLine($"LOG - {text}");
    }

    internal void Error(string text)
    {
        System.ConsoleColor currentColor = System.Console.ForegroundColor;

        System.Console.ForegroundColor = System.ConsoleColor.Red;
        System.Console.WriteLine($"LOG - {text}");
        System.Console.ForegroundColor = currentColor;
    }
}