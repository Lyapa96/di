using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Core;

namespace TagsCloudApp.DataInput
{
    public class DataSourceParser
    {
        public IEnumerable<string> GetSourceText(string filename)
        {
            var extension = Path.GetExtension(filename);
            var fileParser = Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
            return fileParser.GetFileText(filename);
        }
    }
}