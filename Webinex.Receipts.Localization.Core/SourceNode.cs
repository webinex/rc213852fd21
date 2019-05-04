using System.Collections.Generic;
using System.Linq;

namespace Webinex.Receipts.Localization.Core
{
    public class SourceNode
    {
        public SourceNodeKind Kind { get; }
        
        public string Key { get; }
        
        public string Data { get; }
        
        public SourceNode[] Children { get; }

        public static SourceNode Root(IEnumerable<SourceNode> children)
        {
            return new SourceNode(SourceNodeKind.Root, null, null, children);
        }

        public static SourceNode Nested(string key, IEnumerable<SourceNode> children)
        {
            return new SourceNode(SourceNodeKind.Nested, key, null, children);
        }

        public static SourceNode Leap(string key, string data)
        {
            return new SourceNode(SourceNodeKind.Leap, key, data, null);
        }

        private SourceNode(SourceNodeKind kind, string key, string data, IEnumerable<SourceNode> children)
        {
            Kind = kind;
            Key = key;
            Data = data;
            Children = children?.ToArray();
        }
    }
}