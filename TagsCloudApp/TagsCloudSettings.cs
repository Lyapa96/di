using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
        public delegate TagsCloudSettings Factory(
            int width, int height, string imageFormat, string fontname, Color backgroundColor, IEnumerable<Color> colors,
            Func<WordInformation, Color> algorithmOfColoringWords);

        public int Width { get; set; }
        public int Height { get; set; }
        public string ImageFormat { get; set; }
        public string Fontname { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public Func<WordInformation, Color> AlgorithmOfColoringWords { get; set; }

        public TagsCloudSettings()
        {
            SetDefaultSettings();
        }

        public TagsCloudSettings(int width, int height, string imageFormat, string fontname, Color backgroundColor,
            IEnumerable<Color> colors, Func<WordInformation, Color> algorithmOfColoringWords)
        {
            Width = width;
            Height = height;
            ImageFormat = imageFormat;
            Fontname = fontname;
            BackgroundColor = backgroundColor;
            Colors = colors;
            AlgorithmOfColoringWords = algorithmOfColoringWords;
        }

        private void SetDefaultSettings()
        {
            Width = 1000;
            Height = 1000;
            ImageFormat = ".png";
            Fontname = "Arial";
            BackgroundColor = Color.White;
            Colors = new Color[] {Color.Blue};
            AlgorithmOfColoringWords = w => Colors.ElementAt(0);
        }
    }
}