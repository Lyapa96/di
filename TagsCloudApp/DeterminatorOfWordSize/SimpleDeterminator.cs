using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public class SimpleDeterminator : IDeterminatorOfWordSize
    {
        private const int BigSize = 30;
        private const int MediumSize = 20;
        private const int SmallSize = 10;

        private readonly double averageFrequency;


        public SimpleDeterminator(Dictionary<string, int> wordToFrequency)
        {
            averageFrequency = wordToFrequency.Select(w => w.Value).Average();
        }

        public SimpleDeterminator()
        {
            averageFrequency = 5;
        }

        public Size GetSize(WordInformation word)
        {
            if (word.Frequency > averageFrequency)
                return new Size(word.Content.Length*BigSize, BigSize);
            if (word.Frequency < 0.4*averageFrequency)
                return new Size(word.Content.Length*SmallSize, SmallSize);
            return new Size(word.Content.Length*MediumSize, MediumSize);
        }
    }
}