using System.Data;
using System.Text.RegularExpressions;
using DataMasker;
using Newtonsoft.Json.Linq;

namespace DataMaskerTests
{
    [TestClass]
    public class MaskingUtilsTests
    {
        // Define path to test data file 
        private string testDataPath;

        public MaskingUtilsTests()
        {
            testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestData");
        }

        [TestMethod]

        // Should return empty Rule object
        public void TestMakeRule_EmptyJSON_EmptyLists()
        {
            string filePath = Path.Combine(testDataPath, "empty_rules.json");
            DataMasker.Rule rule = MaskingUtils.MakeRule(filePath);
            Assert.IsNotNull(rule);
            Assert.AreEqual(0, rule.KeyPatterns.Count);
            Assert.AreEqual(0, rule.ValuePatterns.Count);
        }

        [TestMethod]
        // Should return populated Rule object
        public void TestMakeRule_ValidJson_ListsCorrectlyPopulated()
        {
            string filePath = Path.Combine(testDataPath, "valid_rules.json");
            DataMasker.Rule rule = MaskingUtils.MakeRule(filePath);
            Assert.IsNotNull(rule);

            // Key Pattern tests
            Assert.AreEqual(5, rule.KeyPatterns.Count);
            Assert.IsNotNull(rule.GetMatchingPattern("nut", "k"));
            Assert.IsNotNull(rule.GetMatchingPattern("fruit", "k"));
            Assert.IsNotNull(rule.GetMatchingPattern("vegetable", "k"));
            Assert.IsNotNull(rule.GetMatchingPattern("animal", "k"));
            Assert.IsNotNull(rule.GetMatchingPattern("mineral", "k"));
            // Value pattern tests
            Assert.AreEqual(5, rule.ValuePatterns.Count);
            Assert.IsNotNull(rule.GetMatchingPattern("nut", "v"));
            Assert.IsNotNull(rule.GetMatchingPattern("fruit", "v"));
            Assert.IsNotNull(rule.GetMatchingPattern("vegetable", "v"));
            Assert.IsNotNull(rule.GetMatchingPattern("animal", "v"));
            Assert.IsNotNull(rule.GetMatchingPattern("mineral", "v"));
        }

        [TestMethod]
        public void TestLoadData_EmptyJson()
        {
            string jsonFilePath = Path.Combine(testDataPath, "empty_data.json");
            JArray array = MaskingUtils.LoadData(jsonFilePath);
            Assert.IsNotNull(array);
            Assert.AreEqual(0, array.Count);
        }

        [TestMethod]
        public void TestLoadData_ValidJson()
        {
            string jsonFilePath = Path.Combine(testDataPath, "valid_data.json");
            JArray array = MaskingUtils.LoadData(jsonFilePath);
            Assert.IsNotNull(array);
            Assert.AreEqual(2, array.Count);
            Assert.IsInstanceOfType(array[0], typeof(JObject));
            Assert.IsInstanceOfType(array[1], typeof(JObject));
        }
        [TestMethod]
        public void TestMaskData_KeyPatterns()
        {
            // Set up test data
            JArray dataArray = JArray.Parse("[{\"key1\": \"value1\"}, {\"key2\": \"value2\"}]");
            DataMasker.Rule rule = new DataMasker.Rule();
            rule.KeyPatterns.Add(new Regex("^key1$"));

            MaskingUtils.MaskData(dataArray, rule);

            Assert.AreEqual("******", dataArray[0]["key1"].ToString());
            Assert.AreEqual("value2", dataArray[1]["key2"].ToString());

        }
        [TestMethod]
        public void TestMaskData_ValuePatterns()
        {
            // Set up test data
            JArray dataArray = JArray.Parse("[{\"key1\": \"value1\"}, {\"key2\": \"value2\"}]");
            DataMasker.Rule rule = new DataMasker.Rule();
            rule.ValuePatterns.Add(new Regex("^value1$"));

            MaskingUtils.MaskData(dataArray, rule);

            Assert.AreEqual("******", dataArray[0]["key1"].ToString());
            Assert.AreEqual("value2", dataArray[1]["key2"].ToString());

        }
        [TestMethod]
        public void TestMaskData_NoPatterns()
        {
            // Set up test data
            JArray dataArray = JArray.Parse("[{\"key1\": \"value1\"}, {\"key2\": \"value2\"}]");
            DataMasker.Rule rule = new DataMasker.Rule();

            MaskingUtils.MaskData(dataArray, rule);

            Assert.AreEqual("value1", dataArray[0]["key1"].ToString());
            Assert.AreEqual("value2", dataArray[1]["key2"].ToString());

        }
    }
}