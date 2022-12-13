using System;

namespace DataMasker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expected Arguments: [JSON Path] [Rules Path]");
                Console.ReadKey();
            }
            else
            {
                string jsonPath = args[0];
                string rulesPath = args[1];
            }
        }
    }
}