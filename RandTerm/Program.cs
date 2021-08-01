using System;
using System.IO;
using System.Net.NetworkInformation;
using ConsoleLibrary;

namespace RandTerm
{
    class Program
    {

        static void Main(string[] args)
        {

            if (new Ping().Send("www.google.com.mx").Status != IPStatus.Success)
            {
                Console.WriteLine("You do not seem to be connected to the internet!");

                return;
            }

            Init.CheckFiles(new string[] { "help.txt", "helpArguments.json" }, "RandTerm");
            ConEngineMethods options =  new ConEngineMethods(args, new string[] { "List", "Roll" }, '-'); 

            if (args.Length == 0)
            {
                Console.Write("You do not seem to have given any useful arguments to RandTerm. Would you like to get some help? Type Y or Yes below if you would: ");

                string input = Console.ReadLine();

                if ( (string.Equals(input.Trim(), "Y", StringComparison.OrdinalIgnoreCase)) || (string.Equals(input.Trim(), "YES", StringComparison.OrdinalIgnoreCase)))
                    Console.WriteLine(File.ReadAllText("help.txt") + "\n");

            }

        }

    }

}