using System.Collections;
using System.Collections.Generic;

namespace TagsCloudApp
{
    public interface IPreprocessorWords
    {
        Dictionary<string, int> Processing(IEnumerable<string> text);
    }
}