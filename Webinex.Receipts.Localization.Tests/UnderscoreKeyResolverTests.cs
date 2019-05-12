using System.IO;
using Webinex.Receipts.Localization.Core;
using Webinex.Receipts.Localization.Core.KeyResolvers;
using Webinex.Receipts.Localization.Core.Sources;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class UnderscoreKeyResolverTests
    {
        [Fact]
        public void ShouldResolveKeyCorrectly()
        {
            using (var stream = new MemoryStream())
            {
                var source = new FileSource("fake", "en", stream);
                var parser = new UnderscoreKeyResolver();
                var entry = new EntryPath("KEY", new []{ "PATH-1", "PATH-2" }, source);
                var result = parser.Resolve(entry);
                Assert.Equal("PATH-1_PATH-2_KEY", result);
            }
        }
    }
}