using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using TagsCloudApp.Preprocessors;

namespace TagsCloudApp.Tests
{
    public class Preprocessor_Should
    {
        private static readonly TestCaseData[] TextCase =
        {
            new TestCaseData(new List<string>() {"a a a b b c"}).Returns(new Dictionary<string, int>()
            {
                {"a", 3},
                {"b", 2},
                {"c", 1}
            }),
            new TestCaseData(new List<string>() {"a b b c a a","c a b"}).Returns(new Dictionary<string, int>()
            {
                {"a", 4},
                {"b", 3},
                {"c", 2}
            }),
        };

        [TestCaseSource(nameof(TextCase))]
        public Dictionary<string,int> CreateFrequencyWithoutFilters(List<string> text)
        {
            var preprocessor = new Preprocessor(null);

            var stats = preprocessor.Processing(text);

            return stats;
        }
    }
}