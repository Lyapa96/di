using CommandLine;
using CommandLine.Text;

namespace TagsCloudApp.Client.ConsoleClient
{
    internal class Options
    {
        [Option('w', "image_width", DefaultValue = 500, HelpText = "Width of resulting image with tags cloud")]
        public int ImageWidth { get; set; }

        [Option('h', "image_height", DefaultValue = 500, HelpText = "Height of resulting image with tags cloud")]
        public int ImageHeight { get; set; }

        [OptionArray('r', "center_point", HelpText = "Center of spiral")]
        public int[] CenterPoints { get; set; }

        [Option('e', DefaultValue = ".bmp", HelpText = "format image")]
        public string ImageFormat { get; set; }

        [Option('t', "filename", HelpText = "Set source filename", Required = true)]
        public string TextInputFile { get; set; }

        [Option('i', "image_name", HelpText = "Set image name", Required = true)]
        public string ImageOutputFile { get; set; }

        [Option('f', "font", DefaultValue = "Arial", HelpText = "Font of text in the image")]
        public string Fontname { get; set; }

        [Option('b', "background_color", DefaultValue = "black",
            HelpText = "Background color")]
        public string BackgroundColor { get; set; }

        [OptionArray('c', "text_colors", DefaultValue = null,
            HelpText = "Сolors of words in the text")]
        public string[] TextColors { get; set; }

        [Option('d', "determinator", HelpText = "Set determinator of wordsize first or second")]
        public string NameDeterminatorOfWordSize { get; set; }

        [OptionArray('p', "filter", HelpText = "Set filter of wordsize first or second")]
        public string[] FiltersNames { get; set; }

        [Option('a', "algorithm of coloring", HelpText = "Algorithm of coloring")]
        public string AlgorithmName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}