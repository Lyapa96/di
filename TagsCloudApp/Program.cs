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
                "-i", "image",
                "-f", "Arial",
                "-b", "white",
                "-c", "yellow","red","blue","black",
                "-d", "ordinary",
                "-p", "initial_form","without_boring_words",
                "-a", "random"
            };


            CreateContainer();

            try
            {
                Container.Resolve<ConsoleUi.Factory>().Invoke(args).Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
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
            builder.Register<IFileParser>(
                (c, p) =>
                {
                    var extension = p.Named<string>("extension");
                    if (extension == ".txt")
                        return new TxtParser();
                    if (extension == ".doc")
                        return new DocParser();
                    throw new ArgumentException();
                });
        }

        private static void RegisterIDeterminatorOfWordSize(ContainerBuilder builder)
        {
            builder.Register<IDeterminatorOfWordSize>(
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
                    throw new ArgumentException();
                });

            //builder.RegisterType<FreeWordTypesDeterminator>().As<IDeterminatorOfWordSize>();
            //builder.RegisterType<OrdinaryDeterminator>().As<IDeterminatorOfWordSize>();
            //builder.RegisterType<OneBigWordDeterminator>().As<IDeterminatorOfWordSize>();
        }

        private static void RegisterIAlgorithmOfColoring(ContainerBuilder builder)
        {
            builder.Register<IAlgorithmOfColoring>(
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
                    throw new ArgumentException();
                });
        }

        private static void RegisterIFilterWords(ContainerBuilder builder)
        {
            builder.Register<IFilterWords>(
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
                    return null;
                });
        }
    }
}