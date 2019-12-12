class ConsoleOutput : IOutput
{
    public void Output(string text)
    {
        System.Console.WriteLine(text);
    }
}