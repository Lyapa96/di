using System.Collections.Generic;

namespace TagsCloudApp.Preprocessors
{
    public interface IFilterWords
    {
       Result<Dictionary<string, int>> Processing(Dictionary<string, int> stats);
    }
}