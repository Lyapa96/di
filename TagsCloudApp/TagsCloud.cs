using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using TagsCloudApp.CloudLayouter;
using TagsCloudApp.DeterminatorOfWordSize;
using TagsCloudApp.Preprocessors;

namespace TagsCloudApp
{
    public class TagsCloud
    {
        public delegate TagsCloud Factory(IPreprocessorWords preprocessor, TagsCloudSettings settings,
            IDeterminatorOfWordSize determinatorOfWordSize, ICloudLayouter cloudLayouter);


        private Dictionary<string, ImageFormat> formats = new Dictionary<string, ImageFormat>()
        {
            {".png", ImageFormat.Png},
            {".bmp", ImageFormat.Bmp},
        };

        public TagsCloudSettings Settings { get; set; }
        public ICloudLayouter CloudLayouter { get; set; }
        public IDeterminatorOfWordSize DeterminatorOfWordSize { get; set; }
        public IPreprocessorWords Preprocessor { get; set; }

        public TagsCloud(IPreprocessorWords preprocessor, TagsCloudSettings settings,
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
            //CloudLayouter.SetCloudSetting(new CloudLayouterSettings(Settings.Width,Settings.Height,new Point(Settings.Width/2, Settings.Height/2))); 
            var settings = Program.Container.Resolve<CloudLayouterSettings.Factory>()
                .Invoke(Settings.Width, Settings.Height, new Point(Settings.Width/2, Settings.Height/2));
            CloudLayouter.SetCloudSetting(settings);
            var words =
                stats.Select(tuple => new WordInformation(tuple.Key, tuple.Value)).OrderByDescending(x => x.Frequency);

            Bitmap bitmap = new Bitmap(Settings.Width, Settings.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Settings.BackgroundColor);
            foreach (var wordInformation in words)
            {
                var font = DeterminatorOfWordSize.GetFont(wordInformation, Settings.Fontname);
                var size = g.MeasureString(wordInformation.Content, font);

                Rectangle rectangle;
                try
                {
                    rectangle =
                        CloudLayouter.PutNextRectangle(new Size((int) Math.Ceiling(size.Width),
                            (int) Math.Ceiling(size.Height)));
                }
                catch
                {
                    break;
                }
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(wordInformation.Content, font,
                    new SolidBrush(Settings.AlgorithmOfColoringWords(wordInformation)), rectangle);
            }

            CloudLayouter.RemovePlacedRectangles();
            bitmap.Save(path, formats[Settings.ImageFormat]);
        }
    }
}