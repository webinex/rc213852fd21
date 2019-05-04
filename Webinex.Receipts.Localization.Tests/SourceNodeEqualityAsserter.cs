using System;
using System.Linq;
using MoreLinq;
using Webinex.Receipts.Localization.Core;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public static class SourceNodeEqualityAsserter
    {
        public static void AssertNode(SourceNode expected, SourceNode actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.Equal(expected.Kind, actual.Kind);
            Assert.Equal(expected.Key, actual.Key);

            switch (expected.Kind)
            {
                case SourceNodeKind.Leap:
                    AssertLeap(expected, actual);
                    break;
                case SourceNodeKind.Root:
                    AssertRootNodes(expected, actual);
                    break;
                case SourceNodeKind.Nested:
                    AssertNested(expected, actual);
                    break;
                default:
                    throw new ArgumentException($"Unknown expected node type {expected.Kind}");
            }
        } 

        private static void AssertNested(SourceNode expected, SourceNode actual)
        {
            Assert.Equal(SourceNodeKind.Nested, expected.Kind);
            Assert.Equal(SourceNodeKind.Nested, actual.Kind);
            AssertChildren(expected, actual);
        }

        private static void AssertChildren(SourceNode expected, SourceNode actual)
        {
            Assert.Equal(expected.Children.Length, actual.Children.Length);

            var tuples = expected.Children
                .Join(actual.Children, node => node.Key, node => node.Key, Tuple.Create).ToArray();

            Assert.Equal(expected.Children.Length, tuples.Length);
            tuples.ForEach(tuple => AssertNode(tuple.Item1, tuple.Item2));
        }

        private static void AssertLeap(SourceNode expected, SourceNode actual)
        {
            Assert.Equal(SourceNodeKind.Leap, expected.Kind);
            Assert.Equal(SourceNodeKind.Leap, actual.Kind);
            Assert.Equal(expected.Key, actual.Key);
            Assert.Equal(expected.Data, actual.Data);
        }

        private static void AssertRootNodes(SourceNode expected, SourceNode actual)
        {
            Assert.Equal(SourceNodeKind.Root, expected.Kind);
            Assert.Equal(SourceNodeKind.Root, actual.Kind);
            AssertChildren(expected, actual);
        }
    }
}