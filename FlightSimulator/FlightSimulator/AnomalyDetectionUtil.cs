using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSimulator
{
    public class AnomalyDetectionUtil
    {

        public class Line
        {
            private float a, b;
            public Line(float a, float b)
            {
                this.a = a;
                this.b = b;
            }

            public float f(float x)
            {
                return a * x + b;
            }

            public void setA(float value)
            {
                this.a = value;
            }

            public void setB(float value)
            {
                this.b = value;
            }

            public float getA()
            {
                return this.a;
            }

            public float getB()
            {
                return this.b;
            }
        };

        public class Point
        {
            
            private float x, y;
            public Point(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public void setX(float value)
            {
                this.x = value;
            }

            public void setY(float value)
            {
                this.x = value;
            }

            public float getX()
            {
                return this.x;
            }

            public float getY()
            {
                return this.y;
            }
        };

        public float avg(List<float> x)
        {
            float sum = 0;
            for (int i = 0; i < x.Count(); ++i)
            {
                sum += x[i];
            }
            return sum / x.Count();
        }

        // returns the variance of X and Y
        public float var(List<float> x)
        {
            List<float> xSquare = new List<float>();
            for (int i = 0; i < x.Count(); ++i)
            {
                xSquare.Add(x[i] * x[i]);
            }
            return avg(xSquare) - (float)Math.Pow(avg(x), 2);
        }

        // returns the covariance of X and Y
        public float cov(List<float> x, List<float> y)
        {
            List<float> multiplyList = new List<float>();
            for (int i = 0; i < x.Count(); ++i)
            {
                multiplyList.Add(x[i] * y[i]);
            }
            return avg(multiplyList) - avg(x) * avg(y);
        }


        // returns the Pearson correlation coefficient of X and Y
        public float pearson(List<float> x, List<float> y)
        {
            return cov(x, y) / (float)(Math.Sqrt(var(x) * var(y)));
        }

        // performs a linear regression and returns the line equation
        public Line linear_reg(List<Point> points)
        {

            List<float> xArray = new List<float>();
            List<float> yArray = new List<float>();

            for (int i = 0; i < points.Count(); ++i)
            {
                xArray.Add(points[i].getX());
                yArray.Add(points[i].getY());
            }
            float a = cov(xArray, yArray) / var(xArray);
            float b = avg(yArray) - a * avg(xArray);
            return new Line(a, b);
        }

        // returns the deviation between point p and the line equation of the pointsLeft
        public float dev(Point p, List<Point> points)
        {
            Line line = linear_reg(points);
            return dev(p, line);
        }

        // returns the deviation between point p and the line
        public float dev(Point p, Line l)
        {
            float deviation = l.f(p.getX()) - p.getY();
            if (deviation >= 0)
            {
                return deviation;
            }
            return -deviation;
        }
    }
}
