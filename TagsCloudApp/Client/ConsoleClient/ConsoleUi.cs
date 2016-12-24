using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;


namespace TagsCloudApp.Client.ConsoleClient
{
    internal class ConsoleUi : TagsCloudAppUi
    {
        public delegate ConsoleUi Factory(IEnumerable<string> args);

        private IEnumerable<string> args { get; set; }
        public IOptions Options { get; set; }

        public ConsoleUi(IEnumerable<string> args)
        {
            this.args = args;
            Options = new ConsoleOptions();
        }

        public override void Run()
        {
            
            if (!Parser.Default.ParseArguments(args.ToArray(), Options))
            {
                Console.WriteLine("Arguments given are incorrect");                
            }
            
        }

        public void SaveCloud()
        {
            SaveCloud(Options);
        }
    }
}