﻿using System;
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


        public SimpleDeterminator(Lazy<Dictionary<string, int>> wordToFrequency)
        {
            averageFrequency = wordToFrequency.Value.Select(w => w.Value).Average();
        }

        public SimpleDeterminator()
        {
            averageFrequency = 5;
        }

        public Font GetSize(WordInformation word, string fontname)
        {
            if (word.Frequency > averageFrequency)
                return new Font(fontname, BigSize);
            if (word.Frequency < 0.4*averageFrequency)
                return new Font(fontname, SmallSize);
            return new Font(fontname, MediumSize);
        }
    }
}