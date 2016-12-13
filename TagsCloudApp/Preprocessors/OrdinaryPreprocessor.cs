using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudApp.Preprocessors
{
    public class OrdinaryPreprocessor : IPreprocessorWords
    {
        public Dictionary<string, int> Processing(IEnumerable<string> text)
        {
            var stats = new Dictionary<string,int>();
            var words = text.SelectMany(w => Regex.Split(w, @"\W+"))
               .Select(word => word.ToLower()).ToArray();
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
            return stats;
        }
    }
}