using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public class OneBigWordDeterminator : IDeterminatorOfWordSize
    {
        private int maxFriquency;

        public Font GetFont(WordInformation word, string fontname)
        {
            if(word.Frequency==maxFriquency) return new Font(fontname,60);
            return new Font(fontname,8);
        }

        public void SetParameters(Dictionary<string, int> wordStats)
        {
            maxFriquency = wordStats.Max(p => p.Value);
        }
    }
}