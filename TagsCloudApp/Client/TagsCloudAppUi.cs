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
            //var extension = Path.GetExtension(options.Filename);
            //var fileParser = Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
            //return fileParser.GetFileText(options.Filename);

            var text = Result.Of(() => Path.GetExtension(options.Filename),"Неверный формат входного файла")
                .ThenTry(GetFileParser,"Невозможно получить информацию из данного формата")
                .ThenTry(parser => parser.GetFileText(options.Filename),"Не удалось получить текст из файла");
            return text;
        }



        public virtual void SaveCloud(IOptions options)
        {
            var settings = Result.Of(() => GetTagsCloudSettings(options),"Не удалось получить настройки");
            var layouter = Result.Of(() => Program.Container.Resolve<ICloudLayouter>(), "Не опеределен раскладчик облака");
            var determinator = Result.Of(() => GetDeterminatorOfWordSize(options),"Не установлен определитель размера слов");
            var preprocessor = Result.Of(() => GetPreprocessor(options),"Не удалось установить препроцессор");

            var cloud = GetCloud(preprocessor, settings, determinator, layouter);

            var text = GetSourceText(options);
            var path = GetPathToImg(options);
            cloud.GetValueOrThrow().CreateBitmapWithWords(text.GetValueOrThrow(),path);
        }

        private string GetPathToImg(IOptions options)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            return Path.Combine(tagsCloudsApp, options.ImageOutputFile + options.ImageFormat);
        }

        private TagsCloudSettings GetTagsCloudSettings(IOptions options)
        {
            var backgroudColor = TagsCloudSettingsParser.GetColor(options.BackgroundColor);
            var colors = TagsCloudSettingsParser.GetColors(options.TextColors).ToArray();
            var imageFormat = TagsCloudSettingsParser.GetFormat(options.ImageFormat);
            var center = TagsCloudSettingsParser.GetCenter(options.CenterPoints[0], options.CenterPoints[1]);

            var algorithmOfColoring =
                Program.Container.Resolve<IAlgorithmOfColoring>(new NamedParameter("name", options.AlgorithmName));

            return Program.Container.Resolve<TagsCloudSettings.Factory>()
                .Invoke(options.ImageWidth, options.ImageHeight, center, imageFormat, options.Fontname, backgroudColor,
                    colors,
                    algorithmOfColoring);
        }

        private Preprocessor GetPreprocessor(IOptions options)
        {
            var filters = new List<IFilterWords>();
            if (options.FiltersNames != null)
            {
                foreach (var filterName in options.FiltersNames)
                {
                    var filter = Program.Container.Resolve<IFilterWords>(new NamedParameter("name", filterName));
                    filters.Add(filter);
                }
            }
            return Program.Container.Resolve<Preprocessor.Factory>().Invoke(filters);
        }

        private IFileParser GetFileParser(string extension)
        {
            return Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
        }

        private Result<TagsCloud> GetCloud(Result<Preprocessor> preprocessor, Result<TagsCloudSettings> settings, Result<IDeterminatorOfWordSize> determinator, Result<ICloudLayouter> layouter)
        {
            return Program.Container.Resolve<TagsCloud.Factory>()
                .Invoke(preprocessor.GetValueOrThrow(), settings.GetValueOrThrow(), determinator.GetValueOrThrow(), layouter.GetValueOrThrow());
        }

        private IDeterminatorOfWordSize GetDeterminatorOfWordSize(IOptions options)
        {
            return Program.Container.Resolve<IDeterminatorOfWordSize>(new NamedParameter("name",
                options.NameDeterminatorOfWordSize));
        }
    }
}