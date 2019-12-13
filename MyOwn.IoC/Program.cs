namespace MyOwn.IoC
{
    public static class Program
    {
        public static void Main()
        {
            var myOwnIoC = new MyOwnIoc();
            
            myOwnIoC.Register<IInput, ConsoleInput>();
            myOwnIoC.Register<IOutput, ConsoleOutput>();
            myOwnIoC.RegisterSingleton<ILogger, ConsoleLogger>();
            myOwnIoC.Register<ICalculator, Calculator>();

            myOwnIoC.Resolve<ICalculator>().Start();
        }
    }
}
