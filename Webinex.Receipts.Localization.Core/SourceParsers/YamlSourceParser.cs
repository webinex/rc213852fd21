using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using Webinex.Receipts.Localization.Core.KeyResolvers;
using Webinex.Receipts.Localization.Core.Sources;
using YamlDotNet.RepresentationModel;

namespace Webinex.Receipts.Localization.Core.SourceParsers
{
    public class YamlSourceParser : ISourceParser
    {
        private readonly IKeyResolver _keyResolver;

        public YamlSourceParser(IKeyResolver keyResolver)
        {
            _keyResolver = keyResolver ?? throw new ArgumentNullException(nameof(keyResolver));
        }

        public IDictionary<string, string> Parse(ISource source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            return new Parser(_keyResolver, source).Parse();
        }

        private class Parser
        {
            private readonly ISource _source;
            private readonly IKeyResolver _keyResolver;

            public Parser(IKeyResolver keyResolver, ISource source)
            {
                _keyResolver = keyResolver ?? throw new ArgumentNullException(nameof(keyResolver));
                _source = source ?? throw new ArgumentNullException(nameof(source));
            }

            public IDictionary<string, string> Parse()
            {
                var yaml = new YamlStream();
                using (var reader = new StreamReader(_source.Stream))
                {
                    yaml.Load(reader);
                    return VisitYamlTree(yaml);
                }
            }

            private IDictionary<string, string> VisitYamlTree(YamlStream yaml)
            {
                return yaml.Documents.SelectMany(document => VisitRootNode((YamlMappingNode) document.RootNode))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            private IDictionary<string, string> VisitRootNode(YamlMappingNode rootNode)
            {
                return new Dictionary<string, string>(
                    VisitMappingNodeChildren(null, null, rootNode));
            }

            private LinkedList<KeyValuePair<string, string>> VisitNode(NodeDetails details)
            {
                switch (details.Value.NodeType)
                {
                    case YamlNodeType.Scalar:
                        var result = new LinkedList<KeyValuePair<string, string>>();
                        result.AddLast(VisitScalarNode(details));
                        return result;
                    case YamlNodeType.Mapping:
                        return VisitMappingNode(details);
                    default:
                        throw new ArgumentException(
                            $"Invalid {nameof(YamlSourceParser)} source. " +
                            $"Only objects and scalars supported.{Environment.NewLine}" +
                            $"Received: {details.Value.NodeType}");
                }
            }

            private LinkedList<KeyValuePair<string, string>> VisitMappingNode(NodeDetails details)
            {
                return VisitMappingNodeChildren(details);
            }

            private LinkedList<KeyValuePair<string, string>> VisitMappingNodeChildren(NodeDetails details)
            {
                var mappingNode = (YamlMappingNode) details.Value;
                return VisitMappingNodeChildren(GetScalarNodeKey(details.Key), details.Path, mappingNode);
            }

            private LinkedList<KeyValuePair<string, string>> VisitMappingNodeChildren(string key, string[] path, YamlMappingNode value)
            {
                var result = new LinkedList<KeyValuePair<string, string>>();
                var childPath = NodeDetails.NextPath(path, key);
                foreach (var childNodePair in value.Children)
                {
                    VisitNode(new NodeDetails(childNodePair, childPath)).ForEach(item => result.AddLast(item));
                }

                return result;
                
            }

            private KeyValuePair<string, string> VisitScalarNode(NodeDetails details)
            {
                return KeyValuePair.Create(
                    GetEntryKey(details),
                    GetScalarNodeValue(details.Value));
            }

            private string GetScalarNodeValue(YamlNode node)
            {
                if (node is YamlScalarNode scalarNode)
                {
                    return scalarNode.Value;
                }

                throw new ArgumentException(
                    $"Unexpected node type nodes. Expected {nameof(YamlScalarNode)}. Received: {node.GetType().Name}");
            }

            private string GetEntryKey(NodeDetails details)
            {
                return _keyResolver.Resolve(new EntryPath(GetScalarNodeKey(details.Key), details.Path, _source));
            }

            private string GetScalarNodeKey(YamlNode node)
            {
                if (node is YamlScalarNode scalarNode)
                {
                    return scalarNode.Value;
                }

                throw new ArgumentException($"Keys should be scalar nodes. Received: {node.GetType().Name}");
            }
        }

        private class NodeDetails
        {
            public YamlNode Key { get; }

            public YamlNode Value { get; }

            public string[] Path { get; }

            public NodeDetails(KeyValuePair<YamlNode, YamlNode> nodePair, string[] path)
                : this(nodePair.Key, nodePair.Value, path)
            {
            }

            public NodeDetails(YamlNode key, YamlNode value, string[] path)
            {
                Key = key;
                Value = value;
                Path = path ?? new string[0];
            }

            public static string[] NextPath(string[] path, string next)
            {
                if (string.IsNullOrWhiteSpace(next))
                {
                    return path;
                }

                if (path == null)
                {
                    return new[] { next };
                }
                
                var newPath = new string[path.Length + 1];
                path.CopyTo(newPath, 0);
                newPath[newPath.Length - 1] = next;
                return newPath;
            }
        }
    }
}