using System;
using System.Linq;

namespace Webinex.Receipts.Localization.Core
{
    public class UnderscoreKeyResolver : IKeyResolver
    {
        public string Resolve(IEntryPath entryPath)
        {
            entryPath = entryPath ?? throw new ArgumentNullException(nameof(entryPath));
            
            var isEmptyPath = entryPath.Path == null || !entryPath.Path.Any();
            return isEmptyPath ? entryPath.Key : $"{string.Join("_", entryPath.Path)}_{entryPath.Key}";
        }
    }
}