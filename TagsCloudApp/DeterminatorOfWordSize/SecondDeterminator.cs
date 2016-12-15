using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public class SecondDeterminator : IDeterminatorOfWordSize
    {
        private const int MinFontSize = 8;
        private const int MaxFontSize = 80;

        private int maxFriquence;
        private int minFriquence;

        public Font GetFont(WordInformation word, string fontname)
        {
            double fontSize = MinFontSize + (word.Frequency - minFriquence) * (MaxFontSize - MinFontSize) / (maxFriquence - minFriquence);
            return new Font(fontname, (int)Math.Ceiling(fontSize));
        }

        public void SetParameters(Dictionary<string, int> wordStats)
        {
            maxFriquence = wordStats.Max(w => w.Value);
            minFriquence = wordStats.Min(w => w.Value);
        }
    }
}