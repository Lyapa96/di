using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using CommandLine;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.DataInput;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;


namespace TagsCloudApp.Client.ConsoleClient
{
    internal class ConsoleUi : ITagsCloudAppUi
    {
        private readonly Dictionary<string, Action<Options>> commands = new Dictionary<string, Action<Options>>();

        private string namePreprocessor { get; set; }
        private string nameDeterminatorOfWordSize { get; set; }
        private string inputFilename { get; set; }
        private string imageName { get; set; }

        public ConsoleUi()
        {
            DefaultSettings();
            CreateCommands();
        }

        private void DefaultSettings()
        {
            nameDeterminatorOfWordSize = "first";
            namePreprocessor = "first";
            inputFilename = "1.txt";
            imageName = "result.bmp";
        }

        public void Run()
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == null) return;
                var options = new Options();
                var args = line.Split(' ');
                if (Parser.Default.ParseArguments(args, options))
                {
                    commands[args[0]](options);
                };                
            }
        }

        public void SaveCloud()
        {        
            var settings = Program.Container.Resolve<TagsCloudSettings>();
            var layouter = Program.Container.Resolve<ICloudLayouter>();
            var determinator = Program.Container.Resolve<IDeterminatorOfWordSize>(new NamedParameter("name", nameDeterminatorOfWordSize));
            var preprocessor = Program.Container.Resolve<IPreprocessorWords>(new NamedParameter("name", namePreprocessor));
            var cloud = Program.Container.Resolve<TagsCloud.Factory>().Invoke(preprocessor, settings, determinator, layouter);

            var text = Program.Container.Resolve<DataSourceParser>().GetSourceText(inputFilename);
            
            cloud.CreateBitmapWithWords(text,GetPathToImg());
        }

        private string GetPathToImg()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            return Path.Combine(tagsCloudsApp, imageName);
        }

        private void CreateCommands()
        {
            commands.Add("-i",o => imageName = o.ImageOutputFile);
            commands.Add("-t",o => inputFilename = o.TextInputFile);
            commands.Add("-d",o => nameDeterminatorOfWordSize = o.nameDeterminatorOfWordSize);
            commands.Add("-p",o => namePreprocessor = o.namePreprocessor);
            commands.Add("-s", o => SaveCloud());
        }
    }
}