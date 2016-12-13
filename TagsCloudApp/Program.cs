using System.Drawing;
using System.IO;
using System.Reflection;
using Autofac;
using TagsCloudApp.CloudLayouter.CircularCloudLayouter;
using TagsCloudApp.DataInput;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;

namespace TagsCloudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TxtParser>().As<IFileParser>();
            var container = builder.Build();

            var text = container.Resolve<IFileParser>().GetFileText("1.txt");

            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));

            var cloud = new TagsCloud(new OrdinaryPreprocessor(), new TagsCloudSettings(), new SimpleDeterminator(),
                new CircularCloudLayouter(new Point(250, 250), 500, 500));
            cloud = cloud.SetSettings(new TagsCloudSettings()
            {
                BackgroundColor = Color.Yellow
            });
            cloud.CreateBitmapWithWords(text, Path.Combine(tagsCloudsApp, "1.bmp"));


            cloud = cloud.SetSettings(new TagsCloudSettings()
            {
                BackgroundColor = Color.Azure
            });
            cloud = cloud.SetPreprocessor(new PreprocessorWordsInInitialForm());
            cloud.CreateBitmapWithWords(text, Path.Combine(tagsCloudsApp, "2.bmp"));
        }
    }
}