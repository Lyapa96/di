using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp.AlgorithmOfColoring
{
    public class SimilarColoring : IAlgorithmOfColoring
    {
        public Color GetColor(WordInformation words, IEnumerable<Color> availableColors)
        {
            return availableColors.ElementAt(0);
        }
    }
}