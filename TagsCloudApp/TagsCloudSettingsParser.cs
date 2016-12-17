using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudApp
{
    public static class TagsCloudSettingsParser
    {
        private static readonly Dictionary<string, ImageFormat> formats = new Dictionary<string, ImageFormat>()
        {
            {".png", ImageFormat.Png},
            {".bmp", ImageFormat.Bmp},
        };

        private static readonly Dictionary<string, Color> stringToColors = new Dictionary<string, Color>()
        {
            {"blue", Color.Blue},
            {"red", Color.Red},
            {"white", Color.White},
            {"black", Color.Black},
            {"yellow", Color.Yellow},
        };

        public static ImageFormat GetFormat(string formatName)
        {
            return formats[formatName];
        }

        public static Color GetColor(string colorName)
        {
            return stringToColors[colorName];
        }

        public static IEnumerable<Color> GetColors(IEnumerable<string> colorNames)
        {
            return colorNames.Select(x => stringToColors[x]).ToArray();
        }

        public static Point GetCenter(int x, int y)
        {
            return new Point(x, y);
        }
    }
}