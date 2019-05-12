using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Webinex.Receipts.Localization.Core;
using Webinex.Receipts.Localization.Core.SourceParsers;
using Webinex.Receipts.Localization.Core.Sources;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class FilePatternLocalizationSourceTests
    {
        private readonly string _filesDir;

        public FilePatternLocalizationSourceTests()
        {
            var assemblyDir = Path.GetDirectoryName(GetType().Assembly.Location);
            _filesDir = Path.Combine(assemblyDir, "_files");    
        }
        
        [Fact]
        public void ShouldWorksCorrectly()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sourceParserMock = fixture
                .Freeze<Mock<ISourceParser>>();
            sourceParserMock
                .Setup(s => s.Parse(It.IsAny<ISource>()))
                .Returns(new Dictionary<string, string>());

            new FilePatternLocalizationSource(_filesDir, "en", new[]
            {
                "*.en.yaml",
                "*.en.yml"
            }, fixture.Create<ISourceParser>()).Load();

            sourceParserMock.Verify(
                s => s.Parse(CreateSourceVerification("file_1.en.yaml")),
                Times.Once);
            sourceParserMock.Verify(
                s => s.Parse(CreateSourceVerification("file_2.en.yml")),
                Times.Once);
        }

        private ISource CreateSourceVerification(string fileName)
        {
            Expression<Func<ISource, bool>> predicate = source => source.Lang == "en"
                                                                  && source.Metadata["fileName"] == Path.Combine(_filesDir, fileName);
            return It.Is(predicate);
        }

        private IDictionary<string, string> Expected => new Dictionary<string, string>
        {
            ["file_1_en_value-1"] = "value-1",
            ["file_2_en_value-1"] = "value-1"
        };
    }
}