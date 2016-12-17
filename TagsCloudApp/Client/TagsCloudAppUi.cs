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

        public virtual IEnumerable<string> GetSourceText(IOptions options)
        {
            var extension = Path.GetExtension(options.Filename);
            var fileParser = Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
            return fileParser.GetFileText(options.Filename);
        }

        public virtual void SaveCloud(IOptions options)
        {
            var settings = GetTagsCloudSettings(options);
            var layouter = Program.Container.Resolve<ICloudLayouter>();
            var determinator =
                Program.Container.Resolve<IDeterminatorOfWordSize>(new NamedParameter("name",
                    options.NameDeterminatorOfWordSize));
            var preprocessor = GetPreprocessor(options);

            var cloud = Program.Container.Resolve<TagsCloud.Factory>()
                .Invoke(preprocessor, settings, determinator, layouter);

            var text = GetSourceText(options);

            cloud.CreateBitmapWithWords(text, GetPathToImg(options));
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

            var algorithmOfColoring = Program.Container.Resolve<IAlgorithmOfColoring>(new NamedParameter("name", options.AlgorithmName));

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
    }
}