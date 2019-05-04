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
            SourceNode result = new Flattener(input).Do();
            Assert.Equal(SourceNodeKind.Root, result.Kind);
            Assert.Equal(7, result.Children.Length);
            
            AssertItem(result, "leap-root", "value-root");
            AssertItem(result, "nested-1_leap-1-1", "value-1-1");
            AssertItem(result, "nested-2_leap-2-1", "value-2-1");
            AssertItem(result, "nested-2_leap-2-2", "value-2-2");
            AssertItem(result, "nested-3_nested-3-1_leap-3-1-1", "value-3-1-1");
            AssertItem(result, "nested-3_nested-3-2_leap-3-2-1", "value-3-2-1");
            AssertItem(result, "nested-3_nested-3-2_leap-3-2-1", "value-3-2-1");
        }

        private void AssertItem(SourceNode root, string key, string data)
        {
            var item = root.Children.SingleOrDefault(i => i.Key == key);
            Assert.NotNull(item);
            Assert.Equal(data, item.Data);
        }

        private SourceNode Input()
        {
            var rootLeap = SourceNode.Leap("leap-root", "value-root");
            
            var leap11 = SourceNode.Leap("leap-1-1", "value-1-1");
            var nested1 = SourceNode.Nested("nested-1", new[] {leap11});

            var leap21 = SourceNode.Leap("leap-2-1", "value-2-1");
            var leap22 = SourceNode.Leap("leap-2-2", "value-2-2");
            var nested2 = SourceNode.Nested("nested-2", new[] {leap21, leap22});
            
            var leap311 = SourceNode.Leap("leap-3-1-1", "value-3-1-1");
            var leap321 = SourceNode.Leap("leap-3-2-1", "value-3-2-1");
            var leap322 = SourceNode.Leap("leap-3-2-2", "value-3-2-2");
            var nested31 = SourceNode.Nested("nested-3-1", new[] {leap311});
            var nested32 = SourceNode.Nested("nested-3-2", new[] {leap322, leap321});
            var nested3 = SourceNode.Nested("nested-3", new[] {nested31, nested32});
            
            return SourceNode.Root(new[] {nested1, nested2, nested3, rootLeap});
        }
    }
}