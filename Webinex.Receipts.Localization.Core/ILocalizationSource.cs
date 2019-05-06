using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core
{
    public interface ILocalizationSource
    {
        ILocalizationData Load();
    }
}