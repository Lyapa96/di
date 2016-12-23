using System;
using Autofac;
using TagsCloudApp.AlgorithmOfColoring;
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
                "-m","100"
            };


            CreateContainer();
            Container.Resolve<ConsoleUi.Factory>().Invoke(args).Run();
          
        }


        private static void CreateContainer()
        {
            var builder = new ContainerBuilder();

            RegisterIFileParser(builder);
            RegisterIDeterminatorOfWordSize(builder);
            RegisterIAlgorithmOfColoring(builder);
            RegisterIFilterWords(builder);

            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<TagsCloudSettings>().AsSelf();
            builder.RegisterType<Preprocessor>().AsSelf();
            builder.RegisterType<ConsoleUi>().AsSelf();

            builder.RegisterType<TagsCloud>()
                .UsingConstructor(typeof (Preprocessor), typeof (TagsCloudSettings),
                    typeof (IDeterminatorOfWordSize), typeof (ICloudLayouter));

            Container = builder.Build();
        }

        private static void RegisterIFileParser(ContainerBuilder builder)
        {
            builder.Register<Result<IFileParser>>(
                (c, p) =>
                {
                    var extension = p.Named<string>("extension");
                    if (extension == ".txt")
                        return new TxtParser();
                    if (extension == ".doc")
                        return new DocParser();
                    return Result.Fail<IFileParser>("Не удается обработать данный формат текста");
                });
        }

        private static void RegisterIDeterminatorOfWordSize(ContainerBuilder builder)
        {
            builder.Register<Result<IDeterminatorOfWordSize>>(
                (c, p) =>
                {
                    var name = p.Named<string>("name");
                    if (name == "free_word_types")
                    {
                        return new FreeWordTypesDeterminator();
                    }
                    if (name == "ordinary")
                    {
                        return new OrdinaryDeterminator();
                    }
                    if (name == "one_big_word")
                    {
                        return new OneBigWordDeterminator();
                    }
                    return Result.Fail<IDeterminatorOfWordSize>("Не установлен определитель размера слов");
                });
        }

        private static void RegisterIAlgorithmOfColoring(ContainerBuilder builder)
        {
            builder.Register<Result<IAlgorithmOfColoring>>(
                (c, p) =>
                {
                    var name = p.Named<string>("name");
                    if (name == "random")
                    {
                        return new RandomColoring();
                    }
                    if (name == "similar")
                    {
                        return new SimilarColoring();
                    }
                    return Result.Fail<IAlgorithmOfColoring>($"Алгоритма расскарски с таким именем не существует({name})");
                });
        }

        private static void RegisterIFilterWords(ContainerBuilder builder)
        {
            builder.Register<Result<IFilterWords>>(
                (c, p) =>
                {
                    var name = p.Named<string>("name");
                    if (name == "initial_form")
                    {
                        return new FilterWordsInInitialForm();
                    }
                    if (name == "without_boring_words")
                    {
                        return new FilterBoringWords();
                    }
                    if (name == "without_prepositions")
                    {
                        return new FilterPrepositions();
                    }
                    return Result.Fail<IFilterWords>($"Данного фильтра не существует({name})");
                });
        }
    }
}