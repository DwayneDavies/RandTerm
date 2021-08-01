using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using ConsoleLibrary;

namespace RandTerm
{

    public class ConEngineMethods : ConEngine
    {
        public ConEngineMethods(string[] args, string[] argList, char divider = '-') : base(args, argList, divider)
        {

        }

        public Tuple<string[], string, string, string> GetXInts(string input)
        {
            string minimum = "1", sides, times;
            string pattern = @"[0-9]+";
            string[] mc = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
       
            if (mc.Length == 2)
            {
                sides = mc[1];
                times = mc[0];
            }
            else if (mc.Length == 3)
            {
                sides = mc[1];
                times = mc[0];

                if (Int32.Parse(mc[2]) < Int32.Parse(mc[1]))
                    minimum = mc[2];

            }
            else if (mc.Length == 1)
            {
                sides = "6";
                times = mc[0];
            }
            else
            {
                sides = "6";
                times = "1";
            }

            WebClient client = new WebClient();
            string result = client.DownloadString(@"https://www.random.org/integers/?num=" + times + "&min=" + minimum + "&max=" + sides + "&col=1&base=10&format=plain&rnd=new");
            string[] nums = result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            return new Tuple<string[], string, string, string>(nums, sides, times, minimum);
        }

        public bool List(string input)
        {
            Tuple<string[], string, string, string> returnedObject = GetXInts(input);
            string min = returnedObject.Item3;
            string[] nums = returnedObject.Item1;
            string sides = returnedObject.Item2;
            string times = returnedObject.Item3;
            int total = 0;

            Console.WriteLine("Your list of " + times + " numbers from " + min + " to " + sides + ": ");

            for (int i = 0 ; i < nums.Length - 1; i++)
            {
                Console.WriteLine(nums[i]);
                total += Int32.Parse(nums[i]);
            }
   
            Console.WriteLine("The total is: " + total);

            return true;
        }

        public bool Roll( string diceInput)
        {
            Tuple<string[], string, string, string> returnedObject = GetXInts(diceInput);
            string[] nums = returnedObject.Item1;
            string sides = returnedObject.Item2;
            string times = returnedObject.Item3;

            int total = 0;

            foreach (string n in nums)
                if (n.Trim().Length > 0)
                    total = total + Convert.ToInt32(n.Trim());

            Console.WriteLine("The result of rolling " + times + "d" + sides + " is : " + total);

            return true;
        }

    }

}
