using System.Collections.Generic;
using Webinex.Receipts.Localization.Core.Sources;

namespace Webinex.Receipts.Localization.Core.SourceParsers
{
    public interface ISourceParser
    {
        IDictionary<string, string> Parse(ISource source);
    }
}