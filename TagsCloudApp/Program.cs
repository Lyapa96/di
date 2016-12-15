using System;
using Autofac;
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
            CreateContainer();
            Start();
        }

        private static void Start()
        {
            try
            {
                Container.Resolve<ITagsCloudAppUi>().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DataSourceParser>().AsSelf();
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
            builder.RegisterType<TagsCloudSettings>().AsSelf().UsingConstructor();
            builder.RegisterType<CloudLayouterSettings>();

            builder.RegisterType<TagsCloud>()
                .UsingConstructor(typeof (IPreprocessorWords), typeof (TagsCloudSettings),
                    typeof (IDeterminatorOfWordSize), typeof (ICloudLayouter));

            builder.RegisterType<ConsoleUi>().As<ITagsCloudAppUi>();

            Container = builder.Build();
        }
    }
}