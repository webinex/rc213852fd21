namespace Webinex.Receipts.Localization.Core.Sources
{
    public interface IEntryPath
    {
        string Key { get; }
        
        string[] Path { get; }
        
        ISource Source { get; }
    }
}