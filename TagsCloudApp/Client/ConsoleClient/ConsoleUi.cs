using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using CommandLine;
using TagsCloudApp.AlgorithmOfColoring;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.DataInput;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;


namespace TagsCloudApp.Client.ConsoleClient
{
    internal class ConsoleUi : ITagsCloudAppUi
    {
        public delegate ConsoleUi Factory(IEnumerable<string> args);

        private IEnumerable<string> args { get; set; }
        private Options options { get; set; }

        public ConsoleUi(IEnumerable<string> args)
        {
            this.args = args;
        }

        public void Run()
        {
            options = new Options();
            if (!Parser.Default.ParseArguments(args.ToArray(), options))
            {
                Console.WriteLine("Arguments given are incorrect");
                return;
            }
            SaveCloud();
        }

        public IEnumerable<string> GetSourceText(string filename)
        {
            var extension = Path.GetExtension(filename);
            var fileParser = Program.Container.Resolve<IFileParser>(new NamedParameter("extension", extension));
            return fileParser.GetFileText(filename);
        }

        public void SaveCloud()
        {
            var settings = GetTagsCloudSettings();
            var layouter = Program.Container.Resolve<ICloudLayouter>();
            var determinator =
                Program.Container.Resolve<IDeterminatorOfWordSize>(new NamedParameter("name",
                    options.NameDeterminatorOfWordSize));
            var preprocessor = GetPreprocessor();

            var cloud = Program.Container.Resolve<TagsCloud.Factory>()
                .Invoke(preprocessor, settings, determinator, layouter);

            var text = GetSourceText(options.TextInputFile);

            cloud.CreateBitmapWithWords(text, GetPathToImg());
        }

        private string GetPathToImg()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            return Path.Combine(tagsCloudsApp, options.ImageOutputFile + options.ImageFormat);
        }

        private TagsCloudSettings GetTagsCloudSettings()
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

        private Preprocessor GetPreprocessor()
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