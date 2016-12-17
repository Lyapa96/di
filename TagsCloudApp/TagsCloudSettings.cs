using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudApp.AlgorithmOfColoring;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
        public delegate TagsCloudSettings Factory(
            int width, int height,Point center ,ImageFormat imageFormat, string fontname, Color backgroundColor,
            IEnumerable<Color> colors, IAlgorithmOfColoring algorithmOfColoring);

        public int Width { get; set; }
        public int Height { get; set; }
        public Point CenterPoint { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public string Fontname { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IAlgorithmOfColoring AlgorithmOfColoringWords { get; set; }
        

        public TagsCloudSettings(int width, int height,Point center, ImageFormat imageFormat, string fontname, Color backgroundColor,
            IEnumerable<Color> colors, IAlgorithmOfColoring algorithmOfColoring)
        {
            Width = width;
            Height = height;
            ImageFormat = imageFormat;
            Fontname = fontname;
            CenterPoint = center;
            BackgroundColor = backgroundColor;
            Colors = colors;
            AlgorithmOfColoringWords = algorithmOfColoring;
        }
    }
}