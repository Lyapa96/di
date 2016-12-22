using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TagsCloudApp.Preprocessors
{
    public class FilterPrepositions : IFilterWords
    {
        private string transcriptOfPreposition = "PR";
        private string nameTempFile = "temp.txt";
        private string mystem = "mystem.exe";

        public Result<Dictionary<string, int>> Processing(Dictionary<string, int> stats)
        {
            CreateTempFile(stats);
            var newText = Result.Of(GetTextWithInformation,"Нет доступа к mystem.exe").GetValueOrThrow();

            var words = newText.Split(new [] {"}"}, StringSplitOptions.RemoveEmptyEntries)
                .Where(wordWithInformation => !wordWithInformation.Contains(transcriptOfPreposition))
                .Select(
                    word =>
                    {
                        var index = word.IndexOf("{", StringComparison.Ordinal);
                        return word.Substring(0, index);
                    });
            return words.ToDictionary(word => word, word => stats[word]);
        }

        private string GetTextWithInformation()
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = mystem;
            p.StartInfo.Arguments = $"-i {nameTempFile}";
            p.Start();
            string newText;
            using (var reader = new StreamReader(p.StandardOutput.BaseStream, Encoding.UTF8))
            {
                newText = reader.ReadToEnd();
            }
            p.WaitForExit();
            return newText;
        }

        private void CreateTempFile(Dictionary<string, int> stats)
        {
            var allWords = stats.Keys.ToList();
            File.WriteAllLines(nameTempFile, allWords);
        }
    }
}