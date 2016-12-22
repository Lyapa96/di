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

        public static Result<ImageFormat> GetFormat(string formatName)
        {
            return Result.Of(()=>formats[formatName],$"{formatName} - данный формат изображения не пожддерживается");
        }

        public static Result<Color> GetColor(string colorName)
        {
            return Result.Of(() => stringToColors[colorName], $"{colorName} - данный цвет не поддерживается");
        }

        public static IEnumerable<Result<Color>> GetColors(IEnumerable<string> colorNames)
        {
            return colorNames.Select(GetColor).ToArray();
        }

        public static Point GetCenter(int x, int y)
        {
            return new Point(x, y);
        }
    }
}