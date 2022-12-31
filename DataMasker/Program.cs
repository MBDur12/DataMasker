
using Newtonsoft.Json.Linq;
namespace DataMasker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expected Arguments: [JSON Path] [Rules Path]");
                return;
            }
            else
            {
                // Get file paths
                string dataPath = args[0];
                string rulesPath = args[1];

                // Exit if either file path is not valid
                if (!File.Exists(dataPath) || !File.Exists(rulesPath))
                {
                    Console.WriteLine("One or both of the file paths could not be found.");
                    return;
                }


                Rule rule = MaskingUtils.MakeRule(rulesPath);

                JArray dataArray = MaskingUtils.LoadData(dataPath);

                MaskingUtils.MaskData(dataArray, rule);

                // Build destination path for output file
                string dirName = Path.GetDirectoryName(dataPath);
                string dataFileName = Path.GetFileName(dataPath);

                string outputFilePath  = Path.Combine(dirName, "masked_" + dataFileName);

                // Write new masked data to file
                MaskingUtils.WriteMaskedDataToFile(dataArray, outputFilePath);


            }
        }
    }
}