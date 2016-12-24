using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using TagsCloudApp.AlgorithmOfColoring;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.DataInput;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;

namespace TagsCloudApp.Client
{
    public abstract class TagsCloudAppUi
    {
        public abstract void Run();

        public virtual Result<IEnumerable<string>> GetSourceText(IOptions options)
        {
            var text = Result.Of(() => Path.GetExtension(options.Filename),"Неверный формат входного файла")
                .ThenTry(GetFileParser, "Невозможно получить информацию из данного формата")
                .ThenTry(parser => parser.GetFileText(options.Filename),"Не удалось получить текст из файла");
            return text;
        }

        public virtual void SaveCloud(IOptions options)
        {
            var cloud = Program.Container.Resolve<TagsCloud>();

            var text = GetSourceText(options);

            cloud.CreateBitmapWithWords(text.GetValueOrThrow(), GetPathToImg(options));
        }

       

        private string GetPathToImg(IOptions options)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            return Path.Combine(tagsCloudsApp, options.ImageOutputFile + options.ImageFormat);
        }

        private IFileParser GetFileParser(string extension)
        {
            return Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
        }      
    }
}