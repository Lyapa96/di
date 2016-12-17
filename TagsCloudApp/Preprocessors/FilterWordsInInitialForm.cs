using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;

namespace TagsCloudApp.Preprocessors
{
    public class FilterWordsInInitialForm : IFilterWords
    {
        public Dictionary<string, int> Processing(Dictionary<string, int> stats)
        {
            var newStats = new Dictionary<string, int>();
            using (Hunspell hunspell = new Hunspell("ru_RU.aff", "ru_RU.dic"))
            {
                foreach (var wordToFrequence in stats)
                {
                    var stem = hunspell.Stem(wordToFrequence.Key).FirstOrDefault();
                    stem = stem ?? wordToFrequence.Key;
                    if (newStats.ContainsKey(stem))
                    {
                        newStats[stem] += wordToFrequence.Value;
                    }
                    else
                    {
                        newStats.Add(stem, wordToFrequence.Value);
                    }
                }
            }
            return newStats;
        }
    }
}