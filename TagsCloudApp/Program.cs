using System.IO;
using System.Linq;
using Autofac;
using TagsCloudApp.AlgorithmOfColoring;
using TagsCloudApp.Client;
using TagsCloudApp.Client.ConsoleClient;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.CloudLayouter.CircularCloudLayouter;
using TagsCloudApp.DataInput;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;


namespace TagsCloudApp
{
    class Program
    {
        public static IContainer Container;

        static void Main(string[] args)
        {
            args = new[]
            {
                "-w", "1000",
                "-h", "1000",
                "-r", "500", "500",
                "-e", ".png",
                "-t", "2.txt",
                "-i", "imageWithoutPrepositions",
                "-f", "Times New Roman",
                "-b", "black",
                "-c", "white",
                "-d", "ordinary",
                "-p", "initial_form", "without_prepositions",
                "-a", "random",
                "-m", "100"
            };

            var consoleClient = new ConsoleUi(args);
            consoleClient.Run();
            CreateContainer(consoleClient.Options);
            consoleClient.SaveCloud();
        }


        private static void CreateContainer(IOptions options)
        {
            var builder = new ContainerBuilder();

            RegisterIFileParser(builder, options);
            RegisterIDeterminatorOfWordSize(builder, options);
            RegisterIAlgorithmOfColoring(builder, options);
            RegisterIFilterWords(builder, options);
            RegisterSettings(options, builder);

            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<Preprocessor>().AsSelf();
                        
            builder.Register(c => new TagsCloud(
                Result.Of(() => Container.Resolve<Preprocessor>(), "Не удалось утсановить пердобработчики"),
                Result.Of(() => Container.Resolve<TagsCloudSettings>(), "Неверно заданы настройки"),
                Result.Of(() => Container.Resolve<IDeterminatorOfWordSize>(),
                    "Не удалось утсановить определитель размера слов"),
                Result.Of(() => Container.Resolve<ICloudLayouter>(), "Не удалось утсановить раскладчик")
                ));

            Container = builder.Build();
        }

        private static void RegisterSettings(IOptions options, ContainerBuilder builder)
        {
            var backgroundColor = TagsCloudSettingsParser.GetColor(options.BackgroundColor);
            var colors = TagsCloudSettingsParser.GetColors(options.TextColors).ToArray();
            var imageFormat = TagsCloudSettingsParser.GetFormat(options.ImageFormat);
            var center = TagsCloudSettingsParser.GetCenter(options.CenterPoints[0], options.CenterPoints[1]);


            builder.Register(
                c =>
                    new TagsCloudSettings(options.ImageWidth, options.ImageHeight, center, imageFormat, options.Fontname,
                        backgroundColor, colors
                        , Container.Resolve<IAlgorithmOfColoring>(new NamedParameter("name", options.AlgorithmName)),
                        options.MaxCountWords))
                .As<TagsCloudSettings>();
        }

        private static void RegisterIFileParser(ContainerBuilder builder, IOptions options)
        {
            var extension = Path.GetExtension(options.Filename);

            if (extension == ".txt")
                builder.RegisterType<TxtParser>().AsImplementedInterfaces();
            if (extension == ".doc")
                builder.RegisterType<DocParser>().AsImplementedInterfaces();
        }

        private static void RegisterIDeterminatorOfWordSize(ContainerBuilder builder, IOptions options)
        {
            if (options.NameDeterminatorOfWordSize == "free_word_types")
                builder.RegisterType<FreeWordTypesDeterminator>().AsImplementedInterfaces();
            if (options.NameDeterminatorOfWordSize == "ordinary")
                builder.RegisterType<OrdinaryDeterminator>().AsImplementedInterfaces();
            if (options.NameDeterminatorOfWordSize == "one_big_word")
                builder.RegisterType<OneBigWordDeterminator>().AsImplementedInterfaces();
        }

        private static void RegisterIAlgorithmOfColoring(ContainerBuilder builder, IOptions options)
        {
            if (options.AlgorithmName == "random")
                builder.RegisterType<RandomColoring>().AsImplementedInterfaces();
            if (options.AlgorithmName == "similar")
                builder.RegisterType<SimilarColoring>().AsImplementedInterfaces();
        }

        private static void RegisterIFilterWords(ContainerBuilder builder, IOptions options)
        {
            foreach (var name in options.FiltersNames)
            {
                if (name == "initial_form")
                    builder.RegisterType<FilterWordsInInitialForm>().AsImplementedInterfaces();
                if (name == "without_boring_words")
                    builder.RegisterType<FilterBoringWords>().AsImplementedInterfaces();
                if (name == "without_prepositions")
                    builder.RegisterType<FilterPrepositions>().AsImplementedInterfaces();
            }
        }
    }
}