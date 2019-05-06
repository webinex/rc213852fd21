using System;
using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core
{
    public class LocalizationData : ILocalizationData
    {
        public string Lang { get; }
        public IDictionary<string, string> Data { get; }

        public LocalizationData(string lang, IDictionary<string, string> data)
        {
            Lang = lang ?? throw new ArgumentNullException(nameof(lang));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}