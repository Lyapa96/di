using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class GeometryHelper
    {
        public static double GetDistanceBetweenPoints(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static double GetMaxRadius(Point center, int width, int height)
        {
            var upperLeftCorner = new Point(0, 0);
            var lowerLeftCorner = new Point(0, height);
            var upperRightCorner = new Point(width, 0);
            var lowerRightCorner = new Point(width, height);

            return
                Math.Max(
                    Math.Max(GetDistanceBetweenPoints(center, upperLeftCorner),
                        GetDistanceBetweenPoints(center, upperRightCorner)),
                    Math.Max(GetDistanceBetweenPoints(center, lowerLeftCorner),
                        GetDistanceBetweenPoints(center, lowerRightCorner)));
        }


        public static bool IsIncorrectPoint(Point point, int width, int height)
        {
            return point.X < 0 || point.Y < 0 || point.X > width || point.Y > height;
        }


        public static bool IsRectanglesIntersect(Rectangle r1, Rectangle r2)
        {
            //return r1.IntersectsWith(r2);
            var intersection = Rectangle.Intersect(r1, r2);
            return !(intersection.Width == 0 && intersection.Height == 0);
        }


        public static bool IsRectangleInsideOtherRectangle(Rectangle rectangle, int width, int height)
        {
            var intersection = Rectangle.Intersect(rectangle, new Rectangle(0, 0, width, height));
            return intersection.Width == rectangle.Width && rectangle.Height == intersection.Height;
        }
    }
}