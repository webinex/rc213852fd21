using System.Collections.Generic;
using System.IO;

namespace Webinex.Receipts.Localization.Core.Sources
{
    public interface ISource
    {
        IDictionary<string, string> Metadata { get; }
        
        Stream Stream { get; }
    }
}