using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
        public delegate TagsCloudSettings Factory(
            int width, int height,Point center ,ImageFormat imageFormat, string fontname, Color backgroundColor,
            IEnumerable<Color> colors, Func<WordInformation, Color> algorithmOfColoringWords);

        public int Width { get; set; }
        public int Height { get; set; }
        public Point CenterPoint { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public string Fontname { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public Func<WordInformation, Color> AlgorithmOfColoringWords { get; set; }
        

        public TagsCloudSettings(int width, int height,Point center, ImageFormat imageFormat, string fontname, Color backgroundColor,
            IEnumerable<Color> colors, Func<WordInformation, Color> algorithmOfColoringWords)
        {
            Width = width;
            Height = height;
            ImageFormat = imageFormat;
            Fontname = fontname;
            CenterPoint = center;
            BackgroundColor = backgroundColor;
            Colors = colors;
            AlgorithmOfColoringWords = algorithmOfColoringWords;
        }
    }
}