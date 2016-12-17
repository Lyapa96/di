using System.Collections.Generic;

namespace TagsCloudApp.Preprocessors
{
    public interface IFilterWords
    {
        Dictionary<string, int> Processing(Dictionary<string, int> stats);
    }
}