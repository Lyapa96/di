using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using TagsCloudApp.CloudLayouter.CircularCloudLayouter;
using TagsCloudApp.DataInput;

namespace TagsCloudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new TxtParser();
            var text = parser.GetFileText("1.txt");

            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            
            var cloud = new TagsCloud(text);
            cloud.CreateBirmapWithWords(Path.Combine(tagsCloudsApp, "1.bmp"));
        }
    }
}