using System.Drawing;

namespace TagsCloudApp.CloudLayouter
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        void RemovePlacedRectangles();
    }
}