using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TagsCloudApp.AlgorithmOfColoring;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
        public delegate TagsCloudSettings Factory(
            int width, int height,Point center ,Result<ImageFormat> imageFormat, string fontname, Result<Color> backgroundColor,
            IEnumerable<Result<Color>> colors, IAlgorithmOfColoring algorithmOfColoring,int maxCount);

        public int Width { get; set; }
        public int Height { get; set; }
        public Point CenterPoint { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public string Fontname { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IAlgorithmOfColoring AlgorithmOfColoringWords { get; set; }
        public int MaxCountWords { get; set; }

        public TagsCloudSettings(int width, int height,Point center, Result<ImageFormat> imageFormat, string fontname, Result<Color> backgroundColor,
            IEnumerable<Result<Color>> colors, IAlgorithmOfColoring algorithmOfColoring, int maxCount)
        {
            Width = GetWidth(width).GetValueOrThrow();
            Height = GetHeight(height).GetValueOrThrow();
            Width = width;
            Height = height;
            ImageFormat = imageFormat.GetValueOrThrow();
            Fontname = GetFont(fontname).GetValueOrThrow();
            CenterPoint = GetCenter(Width,Height,center).GetValueOrThrow();
            BackgroundColor = backgroundColor.GetValueOrThrow();
            Colors = colors.Select(c => c.GetValueOrThrow());
            AlgorithmOfColoringWords = algorithmOfColoring;
            MaxCountWords = GetMaxCount(maxCount).GetValueOrThrow();
        }

        private Result<int> GetWidth(int width)
        {
            if (width > 0) return width;
            return Result.Fail<int>("Неверный настройки ширины, ширина должна быть больше 0");
        }

        private Result<int> GetHeight(int height)
        {
            if (height > 0) return height;
            return Result.Fail<int>("Неверный настройки высоты, высота должна быть больше 0");
        }

        private Result<Point> GetCenter(int width,int height, Point centerPoint)
        {
            var center = centerPoint;
            if (center.X < 0 || center.Y < 0 || center.X > width || center.Y > height)
                return Result.Fail<Point>("Точка центра задана некорректно");
            return center;
        }

        private Result<string> GetFont(string fontname)
        {
            if (FontFamily.Families
                .Any(oneFontFamily => oneFontFamily.Name.ToLower().IndexOf(fontname.ToLower(), StringComparison.Ordinal) > -1))
            {
                return fontname;
            }
            return  Result.Fail<string>("Шрифт с таким именем не найден в системе");
        }

        private Result<int> GetMaxCount(int maxCount)
        {
            if (maxCount > 0) return maxCount;
            return Result.Fail<int>("Количсество слов должно быть больше 0");
        }
    }
}