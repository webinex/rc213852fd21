using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core
{
    public interface ISourceParser
    {
        IDictionary<string, string> Parse(ISource source);
    }
}