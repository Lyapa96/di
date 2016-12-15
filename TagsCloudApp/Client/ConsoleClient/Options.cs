using CommandLine;
using CommandLine.Text;

namespace TagsCloudApp.Client.ConsoleClient
{
    internal class Options
    {
        [Option('t', HelpText = "Введите имя файла с текстом")]
        public string TextInputFile { get; set; }

        [Option('i', HelpText = "Введите имя картинки создаваемой картинки")]
        public string ImageOutputFile { get; set; }

        [Option('d', HelpText = "Выберите определитель размера слов first или second")]
        public string nameDeterminatorOfWordSize { get; set; }

        [Option('p', HelpText = "Выберите предобработчик слов first или second")]
        public string namePreprocessor { get; set; }

        [Option('s', HelpText = "Сохранить файл")]
        public string Save { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}