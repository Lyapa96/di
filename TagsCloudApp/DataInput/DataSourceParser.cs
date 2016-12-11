using System;
using System.Collections.Generic;

namespace TagsCloudApp.DataInput
{
    public class DataSourceParser
    {
        private readonly Dictionary<string,IFileParser> extensionToParser = new Dictionary<string, IFileParser>()
        {
            { "txt",new TxtParser()}
        };

        public IEnumerable<string> GetSourceText(string filename)
        {
            var extension = GetExtension(filename);
            var parser = extensionToParser[extension];
            return parser.GetFileText(filename);
        }

        private string GetExtension(string filename)
        {
            if(!filename.Contains(".")) throw new ArgumentException();
            var index = filename.LastIndexOf(".", StringComparison.Ordinal);
            return filename.Substring(index);
        }
    }
}