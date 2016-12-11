using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudApp
{
    public class TagsCloudSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string FileFormat { get; set; }
        public Font Font { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public Func<WordInformation, Color> AlgorithmOfColoringWords { get; set; }
    }
}