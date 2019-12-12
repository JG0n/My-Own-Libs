class ConsoleInput : IInput
{
    private readonly ILogger logger;

    public ConsoleInput(ILogger logger)
    {
        this.logger = logger;
    }

    public int ReadInt()
    {
        string input = System.Console.ReadLine();
        if (int.TryParse(input, out int num))
        {
            return num;
        }

        logger.Log($"Excpeted an integer number but we got: {input}");
        return 0;
    }

    public string ReadString()
    {
        return System.Console.ReadLine();
    }
}