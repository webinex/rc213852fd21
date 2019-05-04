using System.Linq;
using Webinex.Receipts.Localization.Core;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class SourceFlattenTests
    {
        [Fact]
        public void ShouldBeFlattenCorrectly()
        {
            var input = Input();
            var expected = Expected();

            SourceNode result = new Flattener(input).Do();
            SourceNodeEqualityAsserter.AssertNode(expected, result);
        }

        private SourceNode Expected()
        {
            return SourceNode.Root(new[]
            {
                SourceNode.Leap("leap-root", "value-root"),
                SourceNode.Leap("nested-1_leap-1-1", "value-1-1"),
                SourceNode.Leap("nested-2_leap-2-1", "value-2-1"),
                SourceNode.Leap("nested-2_leap-2-2", "value-2-2"),
                SourceNode.Leap("nested-3_nested-3-1_leap-3-1-1", "value-3-1-1"),
                SourceNode.Leap("nested-3_nested-3-2_leap-3-2-1", "value-3-2-1"),
                SourceNode.Leap("nested-3_nested-3-2_leap-3-2-1", "value-3-2-1")
            });
        }

        private SourceNode Input()
        {
            return SourceNode.Root(new[]
            {
                SourceNode.Leap("leap-root", "value-root"),
                SourceNode.Nested("nested-1", new[]
                {
                    SourceNode.Leap("leap-1-1", "value-1-1")
                }),
                SourceNode.Nested("nested-2", new[]
                {
                    SourceNode.Leap("leap-2-1", "value-2-1"),
                    SourceNode.Leap("leap-2-2", "value-2-2")
                }),
                SourceNode.Nested("nested-3", new[]
                {
                    SourceNode.Nested("nested-3-1", new[]
                    {
                        SourceNode.Leap("leap-3-1-1", "value-3-1-1")
                    }),
                    SourceNode.Nested("nested-3-2", new[]
                    {
                        SourceNode.Leap("leap-3-2-1", "value-3-2-1"),
                        SourceNode.Leap("leap-3-2-2", "value-3-2-2")
                    })
                })
            });
        }
    }
}