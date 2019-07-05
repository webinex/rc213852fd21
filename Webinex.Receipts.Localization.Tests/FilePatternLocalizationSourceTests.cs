using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Webinex.Receipts.Localization.Core.SourceParsers;
using Webinex.Receipts.Localization.Core.Sources;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class FilePatternLocalizationSourceTests
    {
        private readonly string _filesDir;
        private readonly IFixture _fixture;

        public FilePatternLocalizationSourceTests()
        {
            var assemblyDir = Path.GetDirectoryName(GetType().Assembly.Location);
            _filesDir = Path.Combine(assemblyDir, "_files");   
            
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        
        [Fact]
        public void ShouldWorksCorrectly()
        {
            var sourceParserMock = _fixture
                .Freeze<Mock<ISourceParser>>();
            sourceParserMock
                .Setup(s => s.Parse(It.IsAny<ISource>()))
                .Returns(new Dictionary<string, string>());

            new FilePatternLocalizationLoader(Path.Combine(_filesDir, "*.en.(yaml|yml)"), "en", _fixture.Create<ISourceParser>()).Load();

            sourceParserMock.Verify(
                s => s.Parse(CreateSourceVerification("file_1.en.yaml")),
                Times.Once);
            sourceParserMock.Verify(
                s => s.Parse(CreateSourceVerification("file_2.en.yml")),
                Times.Once);
        }

        private ISource CreateSourceVerification(string fileName)
        {
            Expression<Func<ISource, bool>> predicate = source => source.Metadata["lang"] == "en"
                                                                  && source.Metadata["fileName"] == Path.Combine(_filesDir, fileName);
            return It.Is(predicate);
        }
    }
}