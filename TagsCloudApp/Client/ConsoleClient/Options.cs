using CommandLine;

namespace TagsCloudApp.Client.ConsoleClient
{
    internal class Options
    {
        [Option('u', "user", HelpText = "Id user")]
        public string UserId { get; set; }
    }
}