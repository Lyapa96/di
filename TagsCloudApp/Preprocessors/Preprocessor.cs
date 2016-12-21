using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloudApp.Preprocessors
{
    public class Preprocessor
    {
        public delegate Preprocessor Factory(IEnumerable<IFilterWords> filters);

        private readonly IEnumerable<IFilterWords> filters;

        public Preprocessor(IEnumerable<IFilterWords> filters)
        {
            if(filters != null)
                this.filters = filters;
            else
                this.filters = new List<IFilterWords>();
        }

        public Dictionary<string, int> Processing(IEnumerable<string> text)
        {
            var stats = new Dictionary<string, int>();
            var words = text.SelectMany(w => Regex.Split(w, @"\W+")).Where(word=> word!="")
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

            foreach (var filter in filters)
            {
                stats = filter.Processing(stats).GetValueOrThrow();
            }
            return stats;
        }
    }
}