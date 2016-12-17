using System.Collections.Generic;
using System.Linq;

namespace TagsCloudApp.Preprocessors
{
    public class FilterBoringWords : IFilterWords
    {
        public Dictionary<string, int> Processing(Dictionary<string, int> stats)
        {
            return stats.Where(wordToFrequence => wordToFrequence.Key.Length > 3).ToDictionary(wordToFrequence => wordToFrequence.Key, wordToFrequence => wordToFrequence.Value);
        }
    }
}