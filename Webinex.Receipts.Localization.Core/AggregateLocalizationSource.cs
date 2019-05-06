using System;
using System.Collections.Generic;
using System.Linq;

namespace Webinex.Receipts.Localization.Core
{
    public class AggregateLocalizationSource : ILocalizationSource
    {
        private IEnumerable<ILocalizationSource> _sources;

        public AggregateLocalizationSource(IEnumerable<ILocalizationSource> sources)
        {
            _sources = sources ?? throw new ArgumentNullException(nameof(sources));

            if (!sources.Any())
            {
                throw new ArgumentException("Cannot be empty", nameof(sources));
            }
        }

        public ILocalizationData Load()
        {
            var loaded = _sources.Select(s => s.Load()).ToArray();
            return null;
        }
    }
}