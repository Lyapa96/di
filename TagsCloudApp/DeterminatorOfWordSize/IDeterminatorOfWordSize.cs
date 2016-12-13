using System.Drawing;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public interface IDeterminatorOfWordSize
    {
        Font GetSize(WordInformation word, string fontname);
    }
}