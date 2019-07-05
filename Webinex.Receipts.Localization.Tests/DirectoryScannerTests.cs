using System.Collections.Generic;
using System.IO;
using System.Linq;
using Webinex.Receipts.Localization.Core.Lang;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    public class DirectoryScannerTests
    {
        private string[] ExpectedResultOfCompleteOptions => new[]
        {
            GetPath("_files", "file_1.en.yaml"),
            GetPath("_files", "file_2.en.yml"),
        };

        private string InputOfCompleteOptions => GetPath("**\\*.en.(yaml|yml)");
        
        [Fact]
        public void CompleteOptionsShouldWorksCorrectly()
        {
            RunTestCase(InputOfCompleteOptions, ExpectedResultOfCompleteOptions);
        }
        
        private string[] ExpectedResultOfCompleteOptionsWithUnixStyle => new[]
        {
            GetPath("_files", "file_1.en.yaml"),
            GetPath("_files", "file_2.en.yml"),
        };

        private string InputOfCompleteOptionsWithUnixStyle =>
            GetPath("**/*.en.(yaml|yml)").Replace("\\", "/");
        
        [Fact]
        public void CompleteOptionsWithUnixStyleShouldWorksCorrectly()
        {
            RunTestCase(InputOfCompleteOptionsWithUnixStyle, ExpectedResultOfCompleteOptionsWithUnixStyle);
        }
        
        
        private string[] ExpectedResultOfExtensionOnlySearch => new[]
        {
            GetPath("_files", "file_1.en.yaml"),
            GetPath("_files", "file_1.en.json"),
        };
        
        private string InputOfExtensionOnlySearch => GetPath("_files\\file_1.en.(yaml|json)");

        [Fact]
        public void ExtensionOnlySearchShouldWorksCorrectly()
        {
            RunTestCase(InputOfExtensionOnlySearch, ExpectedResultOfExtensionOnlySearch);
        }
        
        
        private string[] ExpectedResultOfPatternAndExtensionSearch => new[]
        {
            GetPath("_files", "file_1.en.json"),
            GetPath("_files", "file_2.en.yml"),
        };
        
        private string InputOfPatternAndExtensionSearch => GetPath("_files\\*.en.(yml|json)");

        [Fact]
        public void PatternAndExtensionTestShouldWorksCorrectly()
        {
            RunTestCase(InputOfPatternAndExtensionSearch, ExpectedResultOfPatternAndExtensionSearch);
        }
        
        
        private string[] ExpectedResultOfConcreteFileSearch => new[]
        {
            GetPath("_files", "file_1.en.yaml"),
        };
        
        private string InputOfConcreteFileSearch => GetPath("_files\\file_1.en.yaml");

        [Fact]
        public void ConcreteFileSearchShouldWorksCorrectly()
        {
            RunTestCase(InputOfConcreteFileSearch, ExpectedResultOfConcreteFileSearch);
        }
        
        private string[] ExpectedResultOfPatternOnlySearch => new[]
        {
            GetPath("_files", "file_1.en.yaml"),
            GetPath("_files", "file_1.fr.yaml"),
        };
        
        private string InputOfOfPatternOnlySearch => GetPath("_files\\*.yaml");
        

        [Fact]
        public void PatternOnlySearchShouldWorksCorrectly()
        {
            RunTestCase(InputOfOfPatternOnlySearch, ExpectedResultOfPatternOnlySearch);
        }

        private void RunTestCase(string input, IEnumerable<string> expectedResult)
        {
            var expected = expectedResult.ToArray();
            var scanner = new DirectoryScanner(input);
            var result = scanner.Scan().ToArray();
            Assert.Equal(expected.Length, result.Length);

            foreach (string expectedEntry in expected)
            {
                Assert.Contains(expectedEntry, result);
            }
        }

        private static string GetPath(params string[] paths)
        {
            paths = new[] {Directory.GetCurrentDirectory()}.Concat(paths).ToArray();
            return Path.Combine(paths);
        }
    }
}