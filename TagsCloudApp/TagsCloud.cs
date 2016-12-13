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
            var words =
                Preprocessor.Processing(text)
                    .Select(tuple => new WordInformation(tuple.Key, tuple.Value));

            Bitmap bitmap = new Bitmap(Settings.Width, Settings.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Settings.BackgroundColor);
            foreach (var wordInformation in words)
            {
                var font = DeterminatorOfWordSize.GetSize(wordInformation, Settings.Fontname);
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

            if (Settings.ImageFormat == ".png")
                bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            else
                bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
        }


        public TagsCloud SetPreprocessor(IPreprocessorWords preprocessor)
        {
            return new TagsCloud(preprocessor, Settings, DeterminatorOfWordSize, CloudLayouter);
        }

        public TagsCloud SetDeterminatorOfWordSize(IDeterminatorOfWordSize determinator)
        {
            return new TagsCloud(Preprocessor, Settings, determinator, CloudLayouter);
        }

        public TagsCloud SetSettings(TagsCloudSettings settings)
        {
            return new TagsCloud(Preprocessor, settings, DeterminatorOfWordSize, CloudLayouter);
        }

        public TagsCloud SetSettings(ICloudLayouter cloudLayouter)
        {
            return new TagsCloud(Preprocessor, Settings, DeterminatorOfWordSize, cloudLayouter);
        }
    }
}