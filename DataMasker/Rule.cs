using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataMasker
{
    // Stores Regex patterns read in from a rules JSON file
    public class Rule
    {
        public List<Regex> KeyPatterns { get; set; }

        public List<Regex> ValuePatterns { get; set; }

        public Rule()
        {
            KeyPatterns = new List<Regex>();
            ValuePatterns = new List<Regex>();
        }

        public Regex? GetMatchingPattern(string str, string type)
        {
            List<Regex> patterns;
            if (type == "k")
            {
                patterns = KeyPatterns;
            }
            else
            {
                patterns = ValuePatterns;
            }

            foreach(Regex regex in patterns)
            {
                if (regex.IsMatch(str))
                {
                    return regex;
                }
            }
            return null;
        }
        public string GetMaskedKeyResult(string str)
        {
            string replacedString = "";
            for (int i = 0; i < str.Length; i++)
            {
                replacedString += "*";
            }
            return replacedString;
        }

        public string GetMaskedValueResult(string str, Regex pattern)
        {
            Match match = pattern.Match(str);
            string maskedStr = Regex.Replace(str, match.Value, new string('*', match.Value.Length));
            return maskedStr;
        }
    }
}
