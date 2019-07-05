using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace Webinex.Receipts.Localization.Core.Lang
{
    public class DirectoryScanner
    {
        private static readonly Regex MultipleSearchPatternRegex = new Regex(@"^([^\(]+)\(([a-zA-Z0-9]+\|[a-zA-Z0-9]+)\)$");
        
        private readonly string _pattern;
        
        private string _baseDir;
        private string[] _searchPatterns;
        private SearchOption _searchOption;
        
        public DirectoryScanner(string path)
        {
            _pattern = path ?? throw new ArgumentNullException(nameof(path));

            if (!Path.IsPathRooted(path))
            {
                throw new ArgumentException("Path should be rooted", nameof(path));
            }

            _pattern = Path.GetFullPath(path);
            
            InitSearchPatterns();
            InitDirectory();
        }

        public IEnumerable<string> Scan()
        {
            return _searchPatterns.SelectMany(pattern =>
                Directory.GetFiles(_baseDir, pattern, _searchOption).ToArray()).ToArray();
        }

        private void InitDirectory()
        {
            if (_pattern.Contains("**"))
            {
                _searchOption = SearchOption.AllDirectories;
                _baseDir = _pattern.Substring(0, _pattern.IndexOf("\\**", StringComparison.InvariantCulture) + 1);
                return;
            }

            _searchOption = SearchOption.TopDirectoryOnly;
            _baseDir = _pattern.Substring(0, _pattern.LastIndexOf('\\') + 1);
        }

        private void InitSearchPatterns()
        {
            var index = _pattern.LastIndexOf("\\", StringComparison.InvariantCulture);
            var filePattern = _pattern.Substring(index + 1);
            var result = new LinkedList<string>();

            var matches = MultipleSearchPatternRegex.Match(filePattern);
            if (matches.Success)
            {
                var prefix = matches.Groups[1].Value;
                var splitted = matches.Groups[2].Value.Split('|');
                splitted.ForEach(i => result.AddLast($"{prefix}{i}"));
            }
            else
            {
                result.AddLast(filePattern);
            }

            _searchPatterns = result.ToArray();
        }
    }
}