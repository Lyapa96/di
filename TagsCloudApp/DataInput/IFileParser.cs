using System.Collections.Generic;

namespace TagsCloudApp.DataInput
{
    public interface IFileParser
    {
         IEnumerable<string> GetFileText(string filename);
    }
}