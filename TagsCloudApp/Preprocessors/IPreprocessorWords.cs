using System.Collections.Generic;

namespace TagsCloudApp.Preprocessors
{
    public interface IPreprocessorWords
    {
        Dictionary<string, int> Processing(IEnumerable<string> text);
    }
}