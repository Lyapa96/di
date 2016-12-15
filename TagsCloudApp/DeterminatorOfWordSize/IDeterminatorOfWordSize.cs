using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public interface IDeterminatorOfWordSize
    {
        Font GetFont(WordInformation word, string fontname);
        void SetParameters(Dictionary<string, int> wordStats);
    }
}