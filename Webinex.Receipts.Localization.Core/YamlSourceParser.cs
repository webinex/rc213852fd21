using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Webinex.Receipts.Localization.Core
{
    public class YamlSourceParser : ISourceParser
    {
        public SourceNode Parse(Stream source)
        {
            return new Parser(source).Parse();
        }

        private class Parser
        {
            private readonly Stream _stream;
            private YamlStream _yaml;
            
            public Parser(Stream stream)
            {
                _stream = stream;
            }

            public SourceNode Parse()
            {
                _yaml = new YamlStream();
                using (var reader = new StreamReader(_stream))
                {
                    _yaml.Load(reader);
                    return ParseYamlTree();
                }
            }

            private SourceNode ParseYamlTree()
            {
                var rootNode = (YamlMappingNode)_yaml.Documents[0].RootNode;
                return VisitRootNode(rootNode);
            }

            private SourceNode VisitRootNode(YamlMappingNode rootNode)
            {
                LinkedList<SourceNode> children = VisitMappingNodeChildren(rootNode);
                return SourceNode.Root(children);
            }

            private SourceNode VisitNode(KeyValuePair<YamlNode, YamlNode> nodePair)
            {
                switch (nodePair.Value.NodeType)
                {
                    case YamlNodeType.Scalar:
                        return VisitScalarNode(nodePair);
                    case YamlNodeType.Mapping:
                        return VisitMappingNode(nodePair);
                    default:
                        throw new ArgumentException($"Invalid {nameof(YamlSourceParser)} source. Only objects and scalars supported." + 
                                        $"{Environment.NewLine}Received: {nodePair.Value.NodeType}");
                }
                
            }

            private SourceNode VisitScalarNode(KeyValuePair<YamlNode, YamlNode> nodePair)
            {
                return SourceNode.Leap(
                    GetScalarNodeKey(nodePair.Key),
                    GetScalarNodeValue(nodePair.Value));
            }

            private SourceNode VisitMappingNode(KeyValuePair<YamlNode, YamlNode> nodePair)
            {
                var result = VisitMappingNodeChildren(nodePair.Value);
                return SourceNode.Nested(GetScalarNodeKey(nodePair.Key), result);
            }

            private LinkedList<SourceNode> VisitMappingNodeChildren(YamlNode nodeValue)
            {
                LinkedList<SourceNode> result = new LinkedList<SourceNode>();
                var mappingNode = (YamlMappingNode) nodeValue;
                foreach (var childNodePair in mappingNode.Children)
                {
                    result.AddLast(VisitNode(childNodePair));
                }

                return result;
            }

            private string GetScalarNodeValue(YamlNode node)
            {
                if (node is YamlScalarNode scalarNode)
                {
                    return scalarNode.Value;
                }
                throw new ArgumentException($"Unexpected node type nodes. Expected {nameof(YamlScalarNode)}. Received: {node.GetType().Name}");
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
    }
}