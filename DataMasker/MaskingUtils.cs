using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataMasker
{
    public static class MaskingUtils
    {
        // Reads in rules JSON file and returns a dictionary
        public static Rule MakeRule(string rulesPath)
        {
            string json = File.ReadAllText(rulesPath);
            
            // Returns empty list if rules JSON is empty, else List of Rule objects
            if (string.IsNullOrEmpty(json))
            {
                return new Rule();
            }
            else
            {
                // Parse the strings from the rules JSON
                JArray array = JArray.Parse(json);
               
                List<string> ruleStrs = new List<string>();
                foreach(var token in array)
                {
                    ruleStrs.Add((string)token);
                }

                // Build Rule object from parsed strings
                Rule rule = new Rule();
                foreach (string ruleStr in ruleStrs)
                {
                    int colonInd = ruleStr.IndexOf(':');
                    string key = ruleStr.Substring(0, colonInd);
                    string value = ruleStr.Substring(colonInd + 1);

                    if (key == "k")
                    {
                        rule.KeyPatterns.Add(new Regex(value));
                    }
                    else if (key == "v")
                    {
                        rule.ValuePatterns.Add(new Regex(value));
                    }
                }

                return rule;

            }
            
        }

        // Returns a JArray from the data file
        public static JArray LoadData(string dataPath)
        {
            string json = File.ReadAllText(dataPath);
            if (string.IsNullOrEmpty(json))
            {
                return new JArray();
            }
            
            try
            {
                JArray array = JArray.Parse(json);
                return array;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error reading JSON data file: {ex.Message}");
                return new JArray();
            }

        }

    }
}
