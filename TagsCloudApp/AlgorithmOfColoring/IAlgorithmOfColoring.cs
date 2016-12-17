using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudApp.AlgorithmOfColoring
{
    public interface IAlgorithmOfColoring
    {
        Color GetColor(WordInformation words, IEnumerable<Color> availableColors);
    }
}