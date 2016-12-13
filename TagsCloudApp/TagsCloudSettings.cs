using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
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

        private void SetDefaultSettings()
        {
            Width = 500;
            Height = 500;
            ImageFormat = ".png";
            Fontname = "Arial";
            BackgroundColor = Color.White;
            Colors = new Color[] {Color.Blue};
            AlgorithmOfColoringWords = w => Colors.ElementAt(0);
        }
    }
}