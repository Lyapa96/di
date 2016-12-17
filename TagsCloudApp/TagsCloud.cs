using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;

namespace TagsCloudApp
{
    public class TagsCloud
    {
        public delegate TagsCloud Factory(Preprocessor preprocessor, TagsCloudSettings settings,
            IDeterminatorOfWordSize determinatorOfWordSize, ICloudLayouter cloudLayouter);

        public TagsCloudSettings Settings { get; set; }
        public ICloudLayouter CloudLayouter { get; set; }
        public IDeterminatorOfWordSize DeterminatorOfWordSize { get; set; }
        public Preprocessor Preprocessor { get; set; }

        public TagsCloud(Preprocessor preprocessor, TagsCloudSettings settings,
            IDeterminatorOfWordSize determinatorOfWordSize, ICloudLayouter cloudLayouter)
        {
            Preprocessor = preprocessor;
            Settings = settings;
            CloudLayouter = cloudLayouter;
            DeterminatorOfWordSize = determinatorOfWordSize;
        }

        public void CreateBitmapWithWords(IEnumerable<string> text, string path)
        {
            var stats = Preprocessor.Processing(text);
            DeterminatorOfWordSize.SetParameters(stats);
            CloudLayouter.SetCloudSetting(Settings.Width,Settings.Height,Settings.CenterPoint);
            var words = stats.Select(tuple => new WordInformation(tuple.Key, tuple.Value)).OrderByDescending(x => x.Frequency);

            Bitmap bitmap = new Bitmap(Settings.Width, Settings.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Settings.BackgroundColor);
            foreach (var wordInformation in words)
            {
                var font = DeterminatorOfWordSize.GetFont(wordInformation, Settings.Fontname);
                var size = g.MeasureString(wordInformation.Content, font);
                var bigSize = new Size((int)Math.Ceiling(size.Width),(int)Math.Ceiling(size.Height));
                if (!CloudLayouter.TryPutNextRectangle(bigSize))
                {
                    break;
                }
                var rectangle = CloudLayouter.LastPlacedRectangle;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(wordInformation.Content, font,
                    new SolidBrush(Settings.AlgorithmOfColoringWords.GetColor(wordInformation,Settings.Colors)), rectangle);
            }

            CloudLayouter.RemovePlacedRectangles();
            bitmap.Save(path, Settings.ImageFormat);
        }
    }
}