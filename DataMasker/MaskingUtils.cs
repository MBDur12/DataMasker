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

        // Return a JArray from the data file
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

        public static void MaskData(JArray dataArray, Rule rule)
        {
            // Iterate over each JObject in the dataArray
            foreach (JObject item in dataArray)
            {
                // Iterate over each JProperty in the JObject
                foreach (JProperty prop in item.Properties())
                {
                    // Get the value of the JProperty as a string
                    string value = prop.Value.ToString();

                    // Check is the key of the JProperty has a matching rule 
                    Regex? keyPattern = rule.GetMatchingPattern(prop.Name, "k");
                    if (keyPattern != null)
                    {
                        // Mask the value if there is a matching key pattern
                        string maskedStr = rule.GetMaskedKeyResult(value);
                        prop.Value = maskedStr;
                    }
                    else
                    {
                        // If the key doesn't match a key pattern, check if the value matches a value pattern
                        Regex? valPattern = rule.GetMatchingPattern(value, "v");
                        if (valPattern != null)
                        {
                            // If there is a match, get the masked version of the value
                            string maskedStr = rule.GetMaskedValueResult(value, valPattern);
                            prop.Value = maskedStr;
                        }
                    }
                    
                    

                }
            }
        }

        public static void WriteMaskedDataToFile(JArray dataArray, string filePath)
        {
            string serializedData = JsonConvert.SerializeObject(dataArray);
            File.WriteAllText(filePath, serializedData);
            Console.WriteLine($"Produced new masked file: {filePath}");
        }

    }
}
