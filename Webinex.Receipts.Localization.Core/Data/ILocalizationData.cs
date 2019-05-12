using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core.Data
{
    public interface ILocalizationData
    {
        string Lang { get; }
        
        IDictionary<string, string> Data { get; }
    }
}