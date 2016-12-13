using System.Collections.Generic;
using CommandLine;


namespace TagsCloudApp.Client.ConsoleClient
{
    internal class ConsoleUi : ITagsCloudAppUi
    {
        TagsCloud cloud;
        private string path = "dfgd";
        private IEnumerable<string> text;

        public ConsoleUi(TagsCloud cloud)
        {
            this.cloud = cloud;
        }

        public void Run()
        {
            while (true)
            {
                var line = System.Console.ReadLine();
                if (line == null) return;
                var options = new Options();
                Parser.Default.ParseArguments(line.Split(' '), options);
            }
        }

        public void SaveCloud()
        {
            cloud.CreateBitmapWithWords(text, path);
        }
    }
}