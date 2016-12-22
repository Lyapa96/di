using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
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
            var width = GetWidth(Settings).GetValueOrThrow();
            var height = GetHeight(Settings).GetValueOrThrow();
            var center = GetCenter(Settings).GetValueOrThrow();
            CloudLayouter.SetCloudSetting(width, height, center);
            var words =stats.Select(tuple => new WordInformation(tuple.Key, tuple.Value))
                .OrderByDescending(x => x.Frequency);

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Settings.BackgroundColor);
            foreach (var wordInformation in words)
            {
                var font = GetFont(wordInformation).GetValueOrThrow();
                var size = g.MeasureString(wordInformation.Content, font);
                var bigSize = new Size((int) Math.Ceiling(size.Width), (int) Math.Ceiling(size.Height));
                if (!CloudLayouter.TryPutNextRectangle(bigSize))
                {
                    break;
                }
                var rectangle = CloudLayouter.LastPlacedRectangle;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString(wordInformation.Content, font,
                    new SolidBrush(Settings.AlgorithmOfColoringWords.GetColor(wordInformation, Settings.Colors)),
                    rectangle);
            }

            CloudLayouter.RemovePlacedRectangles();
            bitmap.Save(path, Settings.ImageFormat);
        }

        private Result<Font> GetFont(WordInformation wordInformation)
        {
            return Result.Of(() => DeterminatorOfWordSize.GetFont(wordInformation, Settings.Fontname),
                "Шрифт с таким именем не найден в системе");
        }

        private Result<int> GetWidth(TagsCloudSettings setting)
        {
            if (setting.Width > 0) return Settings.Width;
            return Result.Fail<int>("Неверный настройки ширины, ширина должна быть больше 0");
        }

        private Result<int> GetHeight(TagsCloudSettings setting)
        {
            if (setting.Height > 0) return Settings.Height;
            return Result.Fail<int>("Неверный настройки высоты, высота должна быть больше 0");
        }

        private Result<Point> GetCenter(TagsCloudSettings setting)
        {
            var center = Settings.CenterPoint;
            if (center.X < 0 || center.Y < 0 || center.X > setting.Width || center.Y > setting.Height)
                return Result.Fail<Point>("Точка центра задана некорректно");
            return center;
        }
    }
}