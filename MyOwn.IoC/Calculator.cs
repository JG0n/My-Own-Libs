class Calculator : ICalculator
{
    ILogger logger;
    IInput input;
    IOutput output;
    
    public Calculator(ILogger logger, IInput input, IOutput output)
    {
        // this.logger = new ConsoleLogger();
        // this.input = new ConsoleInput();
        // this.output = new ConsoleOutput();

        this.logger = logger;
        this.input = input;
        this.output = output;
    }

    public void Start()
    {
        output.Output("Enter 1st integer number:");
        int num1 = input.ReadInt();

        output.Output("Enter 2nd integer number:");
        int num2 = input.ReadInt();

        output.Output("Enter operation (+-/*):");
        string op = input.ReadString().Trim();

        int result = 0;
        switch (op)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "/":
                result = num1 / num2;
                break;
            case "*":
                result = num1 * num2;
                break;
            default:
                logger.Log($"Unsupported operator: {op}");
                break;
        }

        output.Output($"The result is: {result}");
        output.Output("Calculator ran out of memory. Shutting down.");
    }
}