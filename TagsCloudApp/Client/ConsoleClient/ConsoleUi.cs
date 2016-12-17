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
      
        public ConsoleUi(IEnumerable<string> args)
        {
            this.args = args;
        }

        public override void Run()
        {
            var consoleOptions = new ConsoleOptions();
            if (!Parser.Default.ParseArguments(args.ToArray(), consoleOptions))
            {
                Console.WriteLine("Arguments given are incorrect");
                return;
            }
            SaveCloud(consoleOptions);
        }
    }
}