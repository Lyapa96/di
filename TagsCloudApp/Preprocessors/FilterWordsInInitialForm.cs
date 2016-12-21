using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;

namespace TagsCloudApp.Preprocessors
{
    public class FilterWordsInInitialForm : IFilterWords
    {
        public Result<Dictionary<string, int>> Processing(Dictionary<string, int> stats)
        {
            var res = Result.Of(() => GetNewStats(stats),"одна из внешних библиотек дала сбой(NHunspell не смог найти свои словари)");
            return res;
        }

        private static Dictionary<string, int> GetNewStats(Dictionary<string, int> stats)
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