using System;
using Lab13;

namespace PnConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your expression: ");
            var userInput = Console.ReadLine();
            while (!string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Convert as reversed polish notation or as usual polish notation? (RPN/PN): ");
                switch (Console.ReadLine()?.ToLower())
                {
                    case "rpn":
                    {
                        try
                        {
                            Console.WriteLine($"Converted expression: \n{PolishNotationConverter.ConvertRpn(userInput)}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                    }
                    case "pn":
                    {
                        try
                        {
                            Console.WriteLine($"Converted expression: \n{PolishNotationConverter.ConvertPn(userInput)}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                    }
                    default: Console.WriteLine("Invalid command."); break;
                }

                Console.WriteLine("Enter your expression: ");
                userInput = Console.ReadLine();
            }
        }
    }
}