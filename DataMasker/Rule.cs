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
        public HashSet<Regex> KeyPatterns { get; set; }

        public HashSet<Regex> ValuePatterns { get; set; }

        public Rule()
        {
            KeyPatterns = new HashSet<Regex>();
            ValuePatterns = new HashSet<Regex>();
        }

    }
}
