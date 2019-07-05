using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Webinex.Receipts.Localization.Core.Data;
using Webinex.Receipts.Localization.Core.Lang;
using Webinex.Receipts.Localization.Core.SourceParsers;

namespace Webinex.Receipts.Localization.Core.Sources
{
    public class FilePatternLocalizationLoader : ILocalizationLoader
    {
        private readonly string _pattern;
        private readonly string _lang;
        private readonly ISourceParser _sourceParser;

        public FilePatternLocalizationLoader(string pattern, string lang, ISourceParser sourceParser)
        {
            _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            _lang = lang ?? throw new ArgumentNullException(nameof(lang));
            _sourceParser = sourceParser;
        }
        
        public ILocalizationData Load()
        {
            string[] filePaths = new DirectoryScanner(_pattern).Scan().ToArray();
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