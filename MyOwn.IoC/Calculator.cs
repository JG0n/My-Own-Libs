using System;
class Calculator : ICalculator
{
    private readonly ILogger logger;
    private readonly IInput input;
    private readonly IOutput output;

    public Calculator(ILogger logger, IInput input, IOutput output)
    {
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

        int result = op switch
        {
            "+" => num1 + num2,
            "-" => num1 - num2,
            "/" => num1 / num2,
            "*" => num1 * num2,
            _ => (new Func<int>(() =>
            {
                logger.Log($"Unsupported operator: {op}");
                return 0;
            }))(),
        };

        output.Output($"The result is: {result}");
        output.Output("Calculator ran out of memory. Shutting down.");
    }
}