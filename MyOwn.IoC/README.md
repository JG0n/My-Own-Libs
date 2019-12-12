# MyOwn.IoC

Simple inversion of control container

## Run sample with all types registred

The code that is commited should run like the displayed below.

```diff
user@host:/workspace/My-Own-Libs/MyOwn.IoC$ dotnet run
!LOG - Singleton created <ILogger,ConsoleLogger>
!LOG - Resolved Singleton <ILogger,ConsoleLogger>
!LOG - Resolved Singleton <ILogger,ConsoleLogger>
!LOG - Resolved <IInput,ConsoleInput>
!LOG - Resolved <IOutput,ConsoleOutput>
!LOG - Resolved <ICalculator,Calculator>
Enter 1st integer number:
2
Enter 2nd integer number:
2
Enter operation (+-/*):
*
The result is: 4
Calculator ran out of memory. Shutting down.
```

## Run with missing registrations

Comment the following like ```myOwnIoC.Register<IInput, ConsoleInput>();```
and you should get the output below.

```diff
user@host:/workspace/My-Own-Libs/MyOwn.IoC$ dotnet run
!LOG - Singleton created <ILogger,ConsoleLogger>
!LOG - Resolved Singleton <ILogger,ConsoleLogger>
-LOG -  Type not registered <IInput>
LOG - Terminating...
```