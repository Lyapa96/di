using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Word;


namespace TagsCloudApp.DataInput
{
    public class DocParser : IFileParser
    {
        public IEnumerable<string> GetFileText(string filename)
        {
            var docPath = GetPathToDoc(filename);
            var app = new Application();
            var doc = app.Documents.Open(docPath);

            var allWords = doc.Content.Text.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            doc.Close();
            app.Quit();

            return allWords;
        }

        private string GetPathToDoc(string filename)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var tagsCloudsApp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
            return Path.Combine(tagsCloudsApp, filename);
        }
    }
}