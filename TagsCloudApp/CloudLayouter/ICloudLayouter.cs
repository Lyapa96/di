using System.Drawing;

namespace TagsCloudApp.CloudLayouter
{
    public interface ICloudLayouter
    {
        Rectangle LastPlacedRectangle { get; }
        Result<bool> TryPutNextRectangle(Size rectangleSize);
        void RemovePlacedRectangles();
        void SetCloudSetting(int width, int height, Point center);
    }
}