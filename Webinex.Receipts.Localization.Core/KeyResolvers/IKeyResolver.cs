using Webinex.Receipts.Localization.Core.Sources;

namespace Webinex.Receipts.Localization.Core.KeyResolvers
{
    public interface IKeyResolver
    {
        string Resolve(IEntryPath entryPath);
    }
}