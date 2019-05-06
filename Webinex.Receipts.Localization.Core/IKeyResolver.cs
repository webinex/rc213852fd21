namespace Webinex.Receipts.Localization.Core
{
    public interface IKeyResolver
    {
        string Resolve(IEntryPath entryPath);
    }
}