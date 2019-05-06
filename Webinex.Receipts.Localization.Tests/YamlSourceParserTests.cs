using System.Collections.Generic;
using System.IO;
using System.Text;
using Webinex.Receipts.Localization.Core;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class YamlSourceParserTests
    {
        [Fact]
        public void YamlParserShouldParseCorrectly()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Document)))
            {
                var keyResolver = new UnderscoreKeyResolver();
                var source = new FileSource("file.yml", "en", stream);
                var result = new YamlSourceParser(keyResolver).Parse(source);
                DictionaryAsserter.Equal(ExpectedResult, result);
            }
        }

        private static readonly IDictionary<string, string> ExpectedResult = new Dictionary<string, string>
        {
            ["nested-1_key-1-1"] = "value-1-1",
            ["nested-2_nested-2-1_key-2-1-1"] = "value-2-1-1",
            ["nested-2_nested-2-2_key-2-2-1"] = "value-2-2-1",
            ["nested-2_nested-2-2_key-2-2-2"] = "value-2-2-2",
            ["key-root"] = "value-root"
        };

        private const string Document = @"
            nested-1:
                key-1-1: value-1-1
            nested-2:
                nested-2-1:
                    key-2-1-1: value-2-1-1
                nested-2-2:
                    key-2-2-1: value-2-2-1
                    key-2-2-2: value-2-2-2
            key-root: value-root
";
    }
}