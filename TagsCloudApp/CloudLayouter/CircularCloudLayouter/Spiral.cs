using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Spiral
    {
        private Point center;

        private readonly double deltaInDegrees;
        private readonly double deltaFromDensity;
        private double currentDensityOfSpiral;
        private double currentAngle;
        public double CurrentRadius { get; private set; }

        public readonly double MaxRadius;

        public Spiral(Point center, int width, int height, double currentDensityOfSpiral,
            double deltaInDegrees)
        {
            this.center = center;
            this.deltaInDegrees = deltaInDegrees;
            this.currentDensityOfSpiral = currentDensityOfSpiral;
            CurrentRadius = currentAngle*currentDensityOfSpiral;
            deltaFromDensity = currentDensityOfSpiral*0.1;
            MaxRadius = GeometryHelper.GetMaxRadius(center, width, height);
        }

        public Point GetNextPoint()
        {
            if (CurrentRadius > MaxRadius) throw new IndexOutOfRangeException();
            currentAngle += deltaInDegrees;
            currentDensityOfSpiral += deltaFromDensity;
            CurrentRadius = currentAngle*currentDensityOfSpiral;

            var x = (int) Math.Round(CurrentRadius*Math.Cos(currentAngle/180*Math.PI)) + center.X;
            var y = (int) Math.Round(CurrentRadius*Math.Sin(currentAngle/180*Math.PI)) + center.Y;

            var nextPoint = new Point(x, y);
            return nextPoint;
        }
    }
}