using System;
using System.Collections.Generic;
using System.IO;

namespace Webinex.Receipts.Localization.Core
{
    public class FileSource : ISource, IDisposable
    {
        public IDictionary<string, string> Metadata { get; }
        
        public string Lang { get; }

        public Stream Stream { get; }

        public FileSource(string fileName, string lang, Stream stream)
        {
            Metadata = new Dictionary<string, string>();
            Lang = lang ?? throw new ArgumentNullException(nameof(lang));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Metadata["fileName"] = fileName;
        }

        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}