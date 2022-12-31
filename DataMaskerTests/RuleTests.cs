using DataMasker;
using System.Text.RegularExpressions;

namespace DataMaskerTests
{
    [TestClass]
    public class RuleTests
    {
        private Rule rule = new Rule();

        [TestInitialize]
        public void Setup()
        {
            // Create a new rule with some test patterns
            rule = new Rule();
            rule.KeyPatterns.Add(new Regex("^key1$"));
            rule.KeyPatterns.Add(new Regex("^key2$"));
            rule.ValuePatterns.Add(new Regex("^value1$"));
            rule.ValuePatterns.Add(new Regex("^value2$"));
        }
        [TestMethod]
        public void TestGetMatchingPattern()
        {
            // Test getting a matching key pattern
            Assert.AreEqual(new Regex("^key1$").ToString(), rule.GetMatchingPattern("key1", "k").ToString());
            Assert.AreEqual(new Regex("^key2$").ToString(), rule.GetMatchingPattern("key2", "k").ToString());

            // Test getting a matching value pattern
            Assert.AreEqual(new Regex("^value1$").ToString(), rule.GetMatchingPattern("value1", "v").ToString());
            Assert.AreEqual(new Regex("^value2$").ToString(), rule.GetMatchingPattern("value2", "v").ToString());

            // Test getting a non-matching pattern
            Assert.IsNull(rule.GetMatchingPattern("nokey", "k"));
            Assert.IsNull(rule.GetMatchingPattern("novalue", "v"));
        }

        [TestMethod]
        public void TestGetMaskedKeyResult()
        {
            // Test masking a key with GetMaskedKeyResult
            Assert.AreEqual("****", rule.GetMaskedKeyResult("key1"));
        }

        [TestMethod]
        public void TestGetMaskedValueResult()
        {
            // Test masking a value with GetMaskedValueResult
            Assert.AreEqual("******", rule.GetMaskedValueResult("value1", new Regex("^value1$")));
            Assert.AreEqual("*****2", rule.GetMaskedValueResult("value2", new Regex("^value")));
        }
    }
}

