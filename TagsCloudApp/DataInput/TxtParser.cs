using System.Collections.Generic;
using System.IO;

namespace TagsCloudApp.DataInput
{
    public class TxtParser : IFileParser
    {
        public IEnumerable<string> GetFileText(string filename)
        {
            return File.ReadLines(filename);
        }
    }
}