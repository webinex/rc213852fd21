using Webinex.Receipts.Localization.Core.Data;

namespace Webinex.Receipts.Localization.Core.Sources
{
    public interface ILocalizationSource
    {
        ILocalizationData Load();
    }
}