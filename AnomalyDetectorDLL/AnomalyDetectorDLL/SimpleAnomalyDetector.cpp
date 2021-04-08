#include "pch.h"
// Author: Aviad Saadi, ID: 212330567
#include "SimpleAnomalyDetector.h"

// deletes all the vector data that has been allocated
void deleteVectorMembers(vector<Point*>& vector) {
    for (int i = 0; i < vector.size(); i++) {
        delete vector[i];
    }
}

// default constructor
SimpleAnomalyDetector::SimpleAnomalyDetector() {
    this->threshold = 0.9;

}

// default destructor
SimpleAnomalyDetector::~SimpleAnomalyDetector() {
    // TODO Auto-generated destructor stub
}

float SimpleAnomalyDetector::getThreshold() {
    return this->threshold;
}

void SimpleAnomalyDetector::setThreshold(float newThreshold) {
    this->threshold = newThreshold;
}

// a function that gets two vectors - a vector of x values and a vector of y values,
// and puts a vector of the corresponding points in the given pointsVector
void SimpleAnomalyDetector::putPointsVector(vector<Point*>& pointsVector, const vector<float>& f1,
    const vector<float>& f2) {
    for (int i = 0; i < f1.size(); i++) {
        pointsVector.push_back(new Point(f1[i], f2[i]));
    }
}

float SimpleAnomalyDetector::calculate_threshold(vector<Point*>& pointsVector, Line& linearReg) {
    float maxDev = 0, currentDev = 0;
    for (int k = 0; k < pointsVector.size(); k++) {
        currentDev = dev(*pointsVector[k], linearReg);
        if (currentDev > maxDev) {
            maxDev = currentDev;
        }
    }
    return maxDev;
}


// this function learns the normal behaviour of a given TimeSeries object
void SimpleAnomalyDetector::learnNormal(const TimeSeries& ts) {
    const vector<string>& propertiesRow = ts.getPropertiesRow();
    vector<float> vectorColumn1, vectorColumn2, maxColumn;
    string propertyNameOfMaxCorrelation;
    float maxCorrelation = 0, absoluteCorrelation = 0;
    float* column1 = nullptr, * column2 = nullptr;
    for (int i = 0; i < propertiesRow.size() - 1; i++) {
        maxCorrelation = 0;
        absoluteCorrelation = 0;
        ts.putCopyOfColumn(propertiesRow[i], vectorColumn1);
        column1 = vectorColumn1.data();
        for (int j = i + 1; j < propertiesRow.size(); j++) {
            ts.putCopyOfColumn(propertiesRow[j], vectorColumn2);
            column2 = vectorColumn2.data();
            int size = vectorColumn1.size();
            absoluteCorrelation = abs(pearson(column1, column2, size));
            if (absoluteCorrelation > maxCorrelation) {
                maxCorrelation = absoluteCorrelation;
                maxColumn = vectorColumn2;
                propertyNameOfMaxCorrelation = propertiesRow[j];
            }
        }
        //if the correlation is greater or equal than the threshold
        // we are adding the correlated features to the cf vector
        createAndAddCorrelatedFeaturesVector(vectorColumn1, maxColumn, propertiesRow[i], propertyNameOfMaxCorrelation,
            maxCorrelation);
    }
}

void SimpleAnomalyDetector::createAndAddCorrelatedFeaturesVector(const vector<float>& currentColumn,
    const vector<float>& maxColumn,
    const string& currentFeatureName,
    const string& maxFeatureName, float maxCorrelation) {
    if (maxCorrelation >= this->threshold) {
        vector<Point*> pointsVector;
        putPointsVector(pointsVector, currentColumn, maxColumn);
        Point** pointsArray = pointsVector.data();
        Line linearReg = linear_reg(pointsArray, pointsVector.size());
        // we find the threshold
        float threshold = calculate_threshold(pointsVector, linearReg);
        deleteVectorMembers(pointsVector); //we delete the allocated memory
        correlatedFeatures cFeatures;
        cFeatures.corrlation = maxCorrelation;
        cFeatures.feature1 = currentFeatureName;
        cFeatures.feature2 = maxFeatureName;
        cFeatures.lin_reg = linearReg;
        cFeatures.threshold = 1.1 * threshold;
        this->cf.push_back(cFeatures);
    }


}

vector<AnomalyReport> SimpleAnomalyDetector::detect(const TimeSeries& ts) {
    vector<AnomalyReport> reportsVector;
    vector<float> column1, column2;
    for (int i = 0; i < this->cf.size(); i++) {
        ts.putCopyOfColumn(cf[i].feature1, column1);
        ts.putCopyOfColumn(cf[i].feature2, column2);
        vector<Point*> pointsVector;
        putPointsVector(pointsVector, column1, column2);
        for (int j = 0; j < pointsVector.size(); j++) {
            if (anomalyCheck(*pointsVector[j], this->cf[i])) { //if we have found aî anomaly
                // we add the anomaly to the reportsVector
                AnomalyReport anomalyReport(cf[i].feature1 + "-" + cf[i].feature2, j + 1);
                reportsVector.push_back(anomalyReport);
            }
        }
        deleteVectorMembers(pointsVector); //we delete the allocated memory
    }
    return reportsVector; //we return the reportsVector

}


bool SimpleAnomalyDetector::anomalyCheck(Point& p, correlatedFeatures& features) {
    float currentDev = dev(p, features.lin_reg);
    return currentDev > features.threshold;
}

extern "C" _declspec(dllexport) void* CreateSimpleAnomalyDetector() {
    return (void*) new SimpleAnomalyDetector();
}


extern "C" _declspec(dllexport) stringWrapper * getCorrelatedFeature(SimpleAnomalyDetector * anomalyDetector, stringWrapper * s) {
    return anomalyDetector->getCorrelatedFeature(s);
}