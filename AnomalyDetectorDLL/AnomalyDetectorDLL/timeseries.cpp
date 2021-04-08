// Author: Aviad Saadi, ID: 212330567

#include "pch.h"
#include "timeseries.h"
#include <fstream>
#include <iostream>
#include <iosfwd>
#include <sstream>

using namespace std;

int TimeSeries::getNumOfRows() {
    return this->numOfRows;
}


//returns the column that corresponds to the given propertyName
const vector<float>& TimeSeries::getColumn(const string& propertyName) const {
    return this->propertyNamesToColumns.find(propertyName)->second;
}

//copies the corresponding column to the given propertyName into columnCopy
void TimeSeries::putCopyOfColumn(const string& propertyName, vector<float>& columnCopy) const {
    columnCopy = getColumn(propertyName);
}

//inserts a new properties row to the property-name-to-column map
void TimeSeries::insertPropertiesRow(const vector<string>& propertiesRow) {
    for (int i = 0; i < this->propertiesRow.size(); i++) {
        auto column = getColumn(this->propertiesRow[i]);
        this->propertyNamesToColumns.erase(this->propertiesRow[i]);
        this->propertyNamesToColumns.insert({ propertiesRow[i], column });
    }
    this->propertiesRow = propertiesRow;
}

//returns the current property row
const vector<string>& TimeSeries::getPropertiesRow() const {
    return this->propertiesRow;
}

// adds a new row - adding a new data to every column based on the given row
void TimeSeries::addRow(const vector<float>& row) {
    for (int i = 0; i < this->propertiesRow.size(); i++) {
        auto& column = this->propertyNamesToColumns.find(this->propertiesRow[i])->second;
        column.push_back(row[i]);
    }
}


//returns the value of the corresponding column of the given property name, at the given row index
float TimeSeries::valueAtTime(const string& propertyName, int rowIndex) const {
    return getColumn(propertyName)[rowIndex - 1]; //we start to count rows from 1
}


//loads a csv file into the property-name-to-column map
void TimeSeries::loadFromCSVFile(const char* CSVfileName) {

    string line, columnName;
    float val;
    ifstream file(CSVfileName);
    //if the file is open and associated with the ifstream object, we continue. otherwise, we throw an exception.
    if (!file.is_open()) throw runtime_error("Couldn't open file");

    if (file.good()) { //if the file is ready to be read
        getline(file, line); //we put the first line of the file in the line variable

        stringstream sstream(line);

        while (getline(sstream, columnName, ',')) {
            this->propertiesRow.push_back(columnName); //set the properties row
            // initialize the empty column of the corresponding property name
            this->propertyNamesToColumns.insert({ columnName, vector<float>{} });

        }
    }


    while (getline(file, line)) { //for every line in the file (excluding the first one)
        stringstream sstream(line);
        this->numOfRows++;
        int colIdx = 0;
        while (sstream >> val) { // if we havn't read all the line data
            //add the value to corresponding column (according to order from left to right)
            this->propertyNamesToColumns.at(this->propertiesRow[colIdx]).push_back(val);
            if (sstream.peek() == ',') sstream.ignore(); // ignore commas
            colIdx++; //increment the column index
        }
    }

    file.close(); //close the file


}

extern "C" _declspec(dllexport) void* CreateTimeSeries() {
    return (void*) new TimeSeries();
}

extern "C" _declspec(dllexport) void* CreateTimeSeriesFromCsv(const char* CSVfileName) {
    return (void*) new TimeSeries(CSVfileName);
}

