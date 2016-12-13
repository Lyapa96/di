﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;

namespace TagsCloudApp.Preprocessors
{
    public class PreprocessorWordsInInitialForm : IPreprocessorWords
    {
        public Dictionary<string, int> Processing(IEnumerable<string> text)
        {
            var stats = new Dictionary<string, int>();
            var words = text.SelectMany(w => Regex.Split(w, @"\W+"))
                .Select(word => word.ToLower()).Where(word => word != "").ToArray();
            foreach (var word in words)
            {
                if (stats.ContainsKey(word))
                {
                    stats[word]++;
                }
                else
                {
                    stats.Add(word, 1);
                }
            }
            return GetNewStats(stats);
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