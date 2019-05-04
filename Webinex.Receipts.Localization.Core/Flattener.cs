using System;
using System.Collections.Generic;
using System.Linq;

namespace Webinex.Receipts.Localization.Core
{
    public class Flattener
    {
        private readonly SourceNode _root;
        
        public Flattener(SourceNode root)
        {
            _root = root ?? throw new ArgumentNullException(nameof(root));
            if (root.Kind != SourceNodeKind.Root)
            {
                throw new ArgumentException("Argument should be root node", nameof(root));
            }
        }

        public SourceNode Do()
        {
            IEnumerable<SourceNode> leaps = ResolveLeaps(_root, null);
            return SourceNode.Root(leaps);
        }

        private IEnumerable<SourceNode> ResolveLeaps(SourceNode node, IList<SourceNode> previous)
        {
            IList<SourceNode> result = new List<SourceNode>();
            
            switch (node.Kind)
            {
                case SourceNodeKind.Leap:
                    result.Add(SourceNode.Leap(ResolveKey(node, previous), node.Data));
                    break;
                case SourceNodeKind.Root:
                case SourceNodeKind.Nested:
                    var nonNullPrevious = previous ?? Enumerable.Empty<SourceNode>();
                    var nextPrevious = nonNullPrevious.Concat(new[] {node}).ToArray();
                    foreach (var childNode in node.Children)
                    {
                        var childLeaps = ResolveLeaps(childNode, nextPrevious);
                        foreach (var childLeap in childLeaps)
                        {
                            result.Add(childLeap);
                        }
                    }
                    break;
                default:
                    throw new ArgumentException($"Unable to flatten node kind {node.Kind}. Key: {node.Key}", nameof(node));
            }

            return result;
        }

        private string ResolveKey(SourceNode node, IList<SourceNode> previous)
        {
            if (previous == null || !previous.Any())
            {
                return node.Key;
            }
            
            var keys = previous.Where(n => n.Kind != SourceNodeKind.Root).Select(n => n.Key).Concat(new []{ node.Key });
            return string.Join("_", keys);
        }
    }
}