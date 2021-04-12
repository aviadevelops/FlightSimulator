using AnomalyDetectorUtil;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FlightSimulator
{
    public class MinCircle
    {
        public class Circle
        {
            private AnomalyDetectionUtil.Point center;
            private float radius;

            public Circle(AnomalyDetectionUtil.Point c, float r)
            {
                this.center = c;
                this.radius = r;
            }

            public AnomalyDetectionUtil.Point getCenter()
            {
                return this.center;
            }

            public float getRadius()
            {
                return this.radius;
            }
        }

        public Circle findMinCircle(AnomalyDetectionUtil.Point[] points, short size)
        {
            List<AnomalyDetectionUtil.Point> boundaryPoints = new List<AnomalyDetectionUtil.Point>();
            return mecAlg(points, boundaryPoints, size);
        }

        // this function calculates the minimal enclosing circle,
        // that is corresponding to the points array and the boundary points
        private Circle mecAlg(AnomalyDetectionUtil.Point[] points, List<AnomalyDetectionUtil.Point> boundaryPoints, short size)
        {
            if (size == 0 || boundaryPoints.Count() == 3)
                return makeCircle(boundaryPoints);
            // If we have an empty array of points,
            // or if we have 3 points we know that are supposed to be on the boundary of the minimal enclosing circle
            // we return the trivial solution that is corresponding to the current boundary points
            // (see documentation of make-circle function)

            short removedIndex = --size;
            // We "remove" the last AnomalyDetectionUtil.Point in the points array
            AnomalyDetectionUtil.Point removedPoint = new AnomalyDetectionUtil.Point(points[removedIndex].getX(), points[removedIndex].getY());
            // We calculate the minimal enclosing circle of the points array after removing the AnomalyDetectionUtil.Point
            Circle currentCircle = mecAlg(points, new List<AnomalyDetectionUtil.Point>(boundaryPoints), size);
            // If the removed AnomalyDetectionUtil.Point is inside the minimal enclosing circle of the remaining points
            if (isInCircle(currentCircle, removedPoint))
                return currentCircle;
            // We have found the minimal enclosing circle that encloses the points that are currently
            // in points array, and we are returning it.
            // Otherwise, we know the removed AnomalyDetectionUtil.Point is supposed to be on the boundary of the minimal enclosing circle,
            // so we add it to the boundaryPoints vector.
            boundaryPoints.Add(removedPoint);
            // And we return the minimal enclosing circle of the updated points array and boundary points vector.
            return mecAlg(points, new List<AnomalyDetectionUtil.Point>(boundaryPoints), size);
        }


        // This function calculates the trivial solution of minimal enclosing circle of 0-3 points.
        private Circle makeCircle(List<AnomalyDetectionUtil.Point> boundaryPoints)
        {
            int size = boundaryPoints.Count();
            if (size == 0) // If we have zero points.
                return new Circle(new AnomalyDetectionUtil.Point(0, 0), 0); // We return a circle with the center (0,0) and with radius 0.
            else if (size == 1) // If we have one AnomalyDetectionUtil.Point.
                return new Circle(boundaryPoints[0], 0); // We return a circle with the given AnomalyDetectionUtil.Point as the center and with radius 0.
            else if (size == 2) // If we have two points.
                // We returns the trivial solution for two points.
                return circleFromTwo(boundaryPoints[0], boundaryPoints[1]);
            else // If we have three points.
                return circleFromThree(boundaryPoints); // We return the trivial solution for three points.
        }

        // This function calculates the trivial solution for a minimal enclosing circle of two given points.
        private Circle circleFromTwo(AnomalyDetectionUtil.Point point1, AnomalyDetectionUtil.Point point2)
        {
            float radius = (float)Math.Sqrt(Math.Pow(point1.getX() - point2.getX(), 2) + Math.Pow(point1.getY() - point2.getY(), 2)) / 2;
            AnomalyDetectionUtil.Point midPoint = new AnomalyDetectionUtil.Point((point1.getX() + point2.getX()) / 2, (point1.getY() + point2.getY()) / 2);
            // We return a circle with the middle AnomalyDetectionUtil.Point of the given two points as it's center,
            // and with the half the distance between the two points as it's radius.
            return new Circle(midPoint, radius);
        }

        // This function calculates the trivial solution for a minimal enclosing circle of the given three points.
        private Circle circleFromThree(List<AnomalyDetectionUtil.Point> p)
        {
            // We try to calculate the trivial solution for a minimal enclosing circle with only two of the three points.
            Circle possCircle1 = circleFromTwo(p[0], p[1]);
            Circle possCircle2 = circleFromTwo(p[0], p[2]);
            Circle possCircle3 = circleFromTwo(p[1], p[2]);
            // If the third AnomalyDetectionUtil.Point is inside the minimal enclosing circle that was created from the other two points,
            // we have found the minimal enclosing circle of the three points.
            if (isInCircle(possCircle1, p[2]))
            {
                return possCircle1;
            }
            else if (isInCircle(possCircle2, p[1]))
            {
                return possCircle2;
            }
            else if (isInCircle(possCircle3, p[0]))
            {
                return possCircle3;
            }
            else // Otherwise, we know the three points have to be on the boundary of the minimal enclosing circle.
                return circleFromThreePoints(p[0], p[1], p[2]);
        }

        // This function calculates the center (that we know three points that are on its boundary) of a circle,
        // we are using some distances between x and y values of these three boundary points.
        private AnomalyDetectionUtil.Point getTempCenterFromThreePoints(float distXP2P1, float distYP2P1, float distXP3P1, float distYP3P1)
        {
            float dest1 = distXP2P1 * distXP2P1 + distYP2P1 * distYP2P1;
            float dest2 = distXP3P1 * distXP3P1 + distYP3P1 * distYP3P1;
            float dest3 = distXP2P1 * distYP3P1 - distYP2P1 * distXP3P1;
            return new AnomalyDetectionUtil.Point((distYP3P1 * dest1 - distYP2P1 * dest2) / (2 * dest3),
                   (distXP2P1 * dest2 - distXP3P1 * dest1) / (2 * dest3));
        }

        // This function calculates a circle that the three given points are on its boundary.
        // (there is only one circle that is defined like this)
        // We use some mathematical calculations of the circle formula that is corresponding all three of the given points.
        private Circle circleFromThreePoints(AnomalyDetectionUtil.Point p1, AnomalyDetectionUtil.Point p2, AnomalyDetectionUtil.Point p3)
        {
            AnomalyDetectionUtil.Point tempCenter = getTempCenterFromThreePoints(p2.getX() - p1.getX(), p2.getY() - p1.getY(), p3.getX() - p1.getX(),
                                                    p3.getY() - p1.getY());
            tempCenter.setX(p1.getX() + tempCenter.getX());
            tempCenter.setY(p1.getY() + tempCenter.getY());
            float radius = (float)Math.Sqrt(Math.Pow(tempCenter.getX() - p1.getX(), 2) + Math.Pow(tempCenter.getY() - p1.getY(), 2));
            return new Circle(tempCenter, radius);
        }

        // This function returns true if the given AnomalyDetectionUtil.Point is inside the given circle, false otherwise.
        private bool isInCircle(Circle c, AnomalyDetectionUtil.Point p)
        {
            // We use the circle formula that determines whatever or not a AnomalyDetectionUtil.Point is inside a circle or on it's boundary.
            return (Math.Pow(p.getX() - c.getCenter().getX(), 2) + Math.Pow(p.getY() - c.getCenter().getY(), 2) <= Math.Pow(c.getRadius(), 2));
        }
    }
}
