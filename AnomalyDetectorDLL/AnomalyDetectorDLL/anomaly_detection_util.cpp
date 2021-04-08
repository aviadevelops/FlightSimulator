/*
 * animaly_detection_util.cpp
 *
 * Author: Aviad Saadi, ID: 212330567
 */

#include "pch.h"
#include <math.h>
#include "anomaly_detection_util.h"

float avg(float* x, int size) {
    float sum = 0;
    for (int i = 0; i < size; ++i) {
        sum += x[i];
    }
    return sum / size;
}

// returns the variance of X and Y
float var(float* x, int size) {
    float* xSquare = new float[size];
    for (int i = 0; i < size; ++i) {
        xSquare[i] = pow(x[i], 2);
    }
    float res = avg(xSquare, size) - pow(avg(x, size), 2);
    delete[] xSquare;
    return  res;
}

// returns the covariance of X and Y
float cov(float* x, float* y, int size) {
    float* multiplyArray = new float[size];
    for (int i = 0; i < size; ++i) {
        multiplyArray[i] = x[i] * y[i];
    }
    float res = avg(multiplyArray, size) - avg(x, size) * avg(y, size);
    delete[] multiplyArray;
    return res;
}


// returns the Pearson correlation coefficient of X and Y
float pearson(float* x, float* y, int size) {
    return cov(x, y, size) / (sqrt(var(x, size) * var(y, size)));
}

// performs a linear regression and returns the line equation
Line linear_reg(Point** points, int size) {

    float* xArray = new float[size], * yArray = new float[size];
    for (int i = 0; i < size; ++i) {
        xArray[i] = points[i]->x;
        yArray[i] = points[i]->y;
    }
    float a = cov(xArray, yArray, size) / var(xArray, size);
    float b = avg(yArray, size) - a * avg(xArray, size);
    delete[] xArray;
    delete[] yArray;
    return Line(a, b);
}

// returns the deviation between point p and the line equation of the points
float dev(Point p, Point** points, int size) {
    Line line = linear_reg(points, size);
    return dev(p, line);
}

// returns the deviation between point p and the line
float dev(Point p, Line l) {
    float deviation = l.f(p.x) - p.y;
    if (deviation >= 0) {
        return deviation;
    }
    return -deviation;
}





