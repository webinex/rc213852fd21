using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Webinex.Receipts.Localization.Core.Data;
using Webinex.Receipts.Localization.Core.SourceParsers;

namespace Webinex.Receipts.Localization.Core.Sources
{
    public class FilePatternLocalizationSource : ILocalizationSource
    {
        private readonly string _path;
        private readonly string _lang;
        private readonly IEnumerable<string> _sourcePatterns;
        private readonly ISourceParser _sourceParser;

        public FilePatternLocalizationSource(string path, string lang, IEnumerable<string> sourcePatterns, ISourceParser sourceParser)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _lang = lang ?? throw new ArgumentNullException(nameof(lang));
            _sourcePatterns = sourcePatterns ?? throw new ArgumentNullException(nameof(sourcePatterns));
            _sourceParser = sourceParser;

            if (!sourcePatterns.Any())
            {
                throw new ArgumentException("Cannot be empty", nameof(sourcePatterns));
            }
        }
        
        public ILocalizationData Load()
        {
            string[] filePaths = _sourcePatterns.SelectMany(pattern => Directory.GetFiles(_path, pattern)).ToArray();
            IList<IDictionary<string, string>> parsedSources = new List<IDictionary<string, string>>(filePaths.Length);
            foreach (string path in filePaths)
            {
                using (var fileStream = File.OpenRead(path))
                {
                    var parsed = _sourceParser.Parse(new FileSource(path, _lang, fileStream));
                    parsedSources.Add(parsed);
                }
            }

            var data = parsedSources.SelectMany(source => source).ToDictionary(s => s.Key, s => s.Value);
            return new LocalizationData(_lang, data);
        }
    }
}