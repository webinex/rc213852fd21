using System;
using System.Linq;
using Webinex.Receipts.Localization.Core.Sources;

namespace Webinex.Receipts.Localization.Core.KeyResolvers
{
    public class UnderscoreKeyResolver : IKeyResolver
    {
        public string Resolve(IEntryPath entryPath)
        {
            entryPath = entryPath ?? throw new ArgumentNullException(nameof(entryPath));

            if (string.IsNullOrWhiteSpace(entryPath.Key))
            {
                throw new ArgumentException(nameof(entryPath), $"{nameof(IEntryPath)}.{nameof(IEntryPath.Key)} cannot be null or empty.");
            }
            
            var isEmptyPath = entryPath.Path == null || !entryPath.Path.Any();
            return isEmptyPath ? entryPath.Key : $"{string.Join("_", entryPath.Path)}_{entryPath.Key}";
        }
    }
}