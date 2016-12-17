using System;
using Autofac;
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
                "-t", "1.txt",
                "-i", "image",
                "-f", "Arial",
                "-b", "white",
                "-c", "red",
                "-d", "first",
                "-p", "first"
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
            builder.Register<IFileParser>(
                (c, p) =>
                {
                    var extension = p.Named<string>("extension");
                    if (extension == ".txt")
                        return new TxtParser();
                    throw new ArgumentException();
                });
            builder.Register<IDeterminatorOfWordSize>(
                (c, p) =>
                {
                    var name = p.Named<string>("name");
                    if (name == "first")
                    {
                        return new FirstDeterminator();
                    }
                    if (name == "second")
                    {
                        return new SecondDeterminator();
                    }
                    throw new ArgumentException();
                });
            builder.Register<IPreprocessorWords>(
                (c, p) =>
                {
                    var name = p.Named<string>("name");
                    if (name == "first")
                    {
                        return new OrdinaryPreprocessor();
                    }
                    if (name == "second")
                    {
                        return new PreprocessorWordsInInitialForm();
                    }
                    throw new ArgumentException();
                });

            builder.RegisterType<FirstDeterminator>().As<IDeterminatorOfWordSize>();
            builder.RegisterType<SecondDeterminator>().As<IDeterminatorOfWordSize>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<PreprocessorWordsInInitialForm>().As<IPreprocessorWords>();
            builder.RegisterType<OrdinaryPreprocessor>().As<IPreprocessorWords>();
            builder.RegisterType<TagsCloudSettings>().AsSelf();


            builder.RegisterType<TagsCloud>()
                .UsingConstructor(typeof (IPreprocessorWords), typeof (TagsCloudSettings),
                    typeof (IDeterminatorOfWordSize), typeof (ICloudLayouter));

            builder.RegisterType<ConsoleUi>().AsSelf();

            Container = builder.Build();
        }
    }
}