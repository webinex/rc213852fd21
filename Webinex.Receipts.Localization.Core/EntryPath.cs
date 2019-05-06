using System;

namespace Webinex.Receipts.Localization.Core
{
    public class EntryPath : IEntryPath
    {
        public string Key { get; }
        
        public string[] Path { get; }
        
        public ISource Source { get; }

        public EntryPath(string key, string[] path, ISource source)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Path = path;
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }
    }
}