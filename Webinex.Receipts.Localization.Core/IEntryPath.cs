namespace Webinex.Receipts.Localization.Core
{
    public interface IEntryPath
    {
        string Key { get; }
        
        string[] Path { get; }
        
        ISource Source { get; }
    }
}