using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public class FirstDeterminator : IDeterminatorOfWordSize
    {
        private const int BigSize = 30;
        private const int MediumSize = 20;
        private const int SmallSize = 10;

        double averageFrequency;

        public Font GetFont(WordInformation word, string fontname)
        {
            if (word.Frequency > averageFrequency)
                return new Font(fontname, BigSize);
            if (word.Frequency < 0.4*averageFrequency)
                return new Font(fontname, SmallSize);
            return new Font(fontname, MediumSize);
        }

        public void SetParameters(Dictionary<string, int> wordStats)
        {
            averageFrequency = wordStats.Select(w => w.Value).Average();
        }
    }
}