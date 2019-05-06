using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core
{
    public interface ILocalizationData
    {
        string Lang { get; }
        
        IDictionary<string, string> Data { get; }
    }
}