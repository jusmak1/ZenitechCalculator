using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ZenitechCalculator
{
    class Program
    {
        private static ILogger _logger;
        private static CalculatorStack _calculatorStack;

        private const int MAX_STACK_SIZE = 5;
        private const int MAX_OPERAND_SIZE_IN_BYTES = 10;

        private static readonly string WELCOME_MESSAGE = "Hello, enter command";
        private static readonly string EXCEEDED_STACK_MESSAGE = "Stack is full";
        private static readonly string NO_REQUIRED_OPERANDS_MESSAGE= "No required operands in stack";
        private static readonly string OPERATION_NOT_FOUND_MESSAGE= "Undefined behaviour";


        private const string PUSH_TEXT = "Push";
        private const string POP_TEXT = "Pop";
        private const string ADD_TEXT = "Add";
        private const string SUBTRACT_TEXT = "Sub";

        static void Main(string[] args)
        {
            Init();

            _logger.LogInformation($"{WELCOME_MESSAGE}");
            while (true)
            {
                var input = Console.ReadLine();
                Calculate(input);
            }
        }

        private static void Calculate(string input)
        {
            var splitedInput = input.Split(' ');

            switch (splitedInput[0])
            {
                case PUSH_TEXT:
                    if (_calculatorStack.Push(uint.Parse(splitedInput[1])) == false)
                            _logger.LogError($"{EXCEEDED_STACK_MESSAGE}");
                    break;
                case POP_TEXT:
                    _calculatorStack.Pop();
                    break;
                case ADD_TEXT:
                    LogOperationResults(_calculatorStack.Add());
                    break;
                case SUBTRACT_TEXT:
                    LogOperationResults(_calculatorStack.Sub());
                    break;
                default:
                    _logger.LogError(OPERATION_NOT_FOUND_MESSAGE);
                    break;
            }

            _logger.LogInformation($"{_calculatorStack.ToString()}");
        }

        private static void LogOperationResults(uint? result)
        {
            if (result == null)
                _logger.LogError($"{NO_REQUIRED_OPERANDS_MESSAGE}");
            else
                _logger.LogInformation($">> {result}");
        }

        private static void Init()
        {
            var ServiceProvider = GetServiceProvider();
            _logger = ServiceProvider.GetService<ILogger<Program>>();

            _calculatorStack = new CalculatorStack(MAX_STACK_SIZE, (int) Math.Pow(2, MAX_OPERAND_SIZE_IN_BYTES));
        }

        private static ServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<Program>();
        }


    }
}
