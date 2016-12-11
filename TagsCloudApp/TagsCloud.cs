using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.CloudLayouter.CircularCloudLayouter;
using TagsCloudApp.DeterminatorOfWordSize;

namespace TagsCloudApp
{
    public class TagsCloud
    {
        public TagsCloudSettings Settings { get; set; }
        public ICloudLayouter CloudLayouter { get; set; }
        public IDeterminatorOfWordSize DeterminatorOfWordSize { get; set; }
        public IPreprocessorWords Preprocessor { get; set; }
        public IEnumerable<string> Text { get; set; }

        public TagsCloud(IEnumerable<string> text)
        {
            Text = text;
            Preprocessor = new OrdinaryPreprocessor();
            Settings = new TagsCloudSettings()
            {
                AlgorithmOfColoringWords = word => Color.Blue,
                Colors = new[] {Color.AliceBlue, Color.Blue},
                FileFormat = ".txt",
                Font = new Font("Arial", 20),
                Height = 500,
                Width = 500
            };
            CloudLayouter = new CircularCloudLayouter(new Point(500/2, 500/2), Settings.Width, Settings.Height);
            DeterminatorOfWordSize = new SimpleDeterminator();
        }

        public void CreateBirmapWithWords(string path)
        {
            var wordToRectangle = GetRectanglesForWords();

            Bitmap bitmap = new Bitmap(Settings.Width, Settings.Height);
            Graphics g = Graphics.FromImage(bitmap);
            foreach (var wordInformation in wordToRectangle.Keys)
            {
                var rectangle = wordToRectangle[wordInformation];
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(wordInformation.Content, Settings.Font,
                    new SolidBrush(Settings.AlgorithmOfColoringWords(wordInformation)), rectangle);
            }
            bitmap.Save(path);
        }

        private Dictionary<WordInformation, Rectangle> GetRectanglesForWords()
        {
            var wordToRectangle = new Dictionary<WordInformation, Rectangle>();
            var words = Preprocessor.Processing(Text).Select(tuple => new WordInformation(tuple.Key, tuple.Value));
            foreach (var word in words)
            {
                var worsSize = DeterminatorOfWordSize.GetSize(word);
                var rectangle = CloudLayouter.PutNextRectangle(worsSize);
                wordToRectangle.Add(word, rectangle);
            }
            return wordToRectangle;
        }
    }
}