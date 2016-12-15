using System.Drawing;

namespace TagsCloudApp.CloudLayouter
{
    public class CloudLayouterSettings
    {
        public delegate CloudLayouterSettings Factory(int width, int height, Point centerPoint);

        public readonly int Width;
        public readonly int Height;
        public readonly Point CenterPoint;

        public CloudLayouterSettings(int width, int height, Point centerPoint)
        {
            Width = width;
            Height = height;
            CenterPoint = centerPoint;
        }
    }
}