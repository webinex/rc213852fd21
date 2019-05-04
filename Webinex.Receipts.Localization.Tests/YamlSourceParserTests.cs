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
                SourceNode result = new YamlSourceParser().Parse(stream);
                SourceNodeEqualityAsserter.AssertNode(ExpectedResult, result);
            }
        }

        private static readonly SourceNode ExpectedResult = SourceNode.Root(new[]
        {
            SourceNode.Nested("nested-1", new[]
            {
                SourceNode.Leap("key-1-1", "value-1-1")
            }),
            SourceNode.Nested("nested-2", new[]
            {
                SourceNode.Nested("nested-2-1", new[]
                {
                    SourceNode.Leap("key-2-1-1", "value-2-1-1")
                }),
                SourceNode.Nested("nested-2-2", new[]
                {
                    SourceNode.Leap("key-2-2-1", "value-2-2-1"),
                    SourceNode.Leap("key-2-2-2", "value-2-2-2")
                })
            }),
            SourceNode.Leap("leap-root", "value-leap-root")
        });

        private const string Document = @"
            nested-1:
                key-1-1: value-1-1
            nested-2:
                nested-2-1:
                    key-2-1-1: value-2-1-1
                nested-2-2:
                    key-2-2-1: value-2-2-1
                    key-2-2-2: value-2-2-2
            leap-root: value-leap-root
";
    }
}