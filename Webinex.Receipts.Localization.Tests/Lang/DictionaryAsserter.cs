using System.Collections.Generic;
using MoreLinq;
using Xunit;

namespace Webinex.Receipts.Localization.Tests
{
    internal static class DictionaryAsserter
    {
        public static void Equal(IDictionary<string, string> expected, IDictionary<string, string> actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            
            expected.ForEach(kv =>
            {
                Assert.True(actual.ContainsKey(kv.Key));
                Assert.Equal(kv.Value, actual[kv.Key]);
            });
        }
    }
}