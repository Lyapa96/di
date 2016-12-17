using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.AlgorithmOfColoring
{
    public class RandomColoring : IAlgorithmOfColoring
    {
        private Random random = new Random();

        public Color GetColor(WordInformation words, IEnumerable<Color> availableColors)
        {
            int max = availableColors.Count();
            return availableColors.ElementAt(random.Next(0, max));
        }
    }
}