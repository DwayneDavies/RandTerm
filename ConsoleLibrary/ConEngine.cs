using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleLibrary
{
    public class ConEngine
    {
        private string[] argList;
        private Dictionary<string, MethodInfo> supportedArgs;
        private Dictionary<string, string> options;

        public int Length { get { return options.Count; } }

        public ConEngine(string[] args, string[] argList, char divider = '-')
        {
            // Don't forget to replace "/" and "--" charactes in our args with "-"! 
            string[] arguments = string.Join(" ", args).Replace('/', '-').Replace("--", "-").Split(divider);
            // Our supported arguments so far. Others are ignored.
            this.argList = argList;
            // Stores options, including some default ones.
            this.options = new Dictionary<string, string>();
            // The supported functions all have their own named methods.
            supportedArgs = new Dictionary<string, MethodInfo>();

            foreach (string arg in argList)
                supportedArgs[arg] = this.GetType().GetTypeInfo().GetDeclaredMethod(arg);

            // Add option, but only if it has a named function that handles that that option does. If it is help, call that instead.
            for (int i = 1; i < arguments.Length; i++)
                if (arguments[i].ToLower().StartsWith("help"))
                    Help(arguments[i]);
                else
                    AddOption(arguments[i].ToLower());

        }

        private void Help(string argument = " ")
        {
            string[] split = argument.Split(" ");

            if ((split.Length < 2))
                if (File.Exists("help.txt"))
                    Console.WriteLine(File.ReadAllText("help.txt") + "\n");
                else
                    Console.WriteLine("Help file has gone missing or is otherwise inaccessible!");
            else
                GetHelpArgument(split[1]);

        }

        private void AddOption(string potentialOption)
        {
            string toSplit = potentialOption;

            // If option has no parameter, this will stop undesired behaviour later.
            if (toSplit.IndexOf(' ') < 0)
                toSplit += " ";

            string[] str = toSplit.Split(' ');
            string arg = FirstLetterToUpper(str[0]);

            if (ArgsContains(argList, arg))
                if ((bool)supportedArgs[arg].Invoke(this, new object[] { str[1] }))
                    options[str[0]] = str[1];

        }

        private string FirstLetterToUpper(string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        private bool ArgsContains(string[] args, string toFind)
        {
            return (Array.FindIndex(args, x => x == toFind) > -1);
        }

        private void GetHelpArgument(string argument)
        {
            string jsonString = System.IO.File.ReadAllText("helpArguments.json");
            Dictionary<string, string> helpArguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            if (helpArguments != null)
                Console.WriteLine("\nHelp for the " + argument + " argument: \n" + helpArguments[argument] + "\n");
            else
                Console.WriteLine($"We do not have any help information for the \"{argument}\" argument\n");

        }

        public string this[string key]
        {
            get { return options[key]; }
        }

    }

}
