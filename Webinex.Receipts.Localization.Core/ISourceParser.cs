using System.IO;

namespace Webinex.Receipts.Localization.Core
{
    public interface ISourceParser
    {
        SourceNode Parse(Stream source);
    }
}