using System.Drawing;

namespace TagsCloudApp.DeterminatorOfWordSize
{
    public interface IDeterminatorOfWordSize
    {
        Size GetSize(WordInformation word);
    }
}