using AnomalyDetectorUtil;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSimulator
{
    public class LinearRegressionAlg
    {
        private static float threshold = (float)0.5;
        private static List<CorrelatedFeatures> cfList = new List<CorrelatedFeatures>();
        private static Dictionary<Tuple<float, float>, long> detectResult;

        private class CorrelatedFeatures
        {
            private int feature1, feature2;  // names of the correlated features
            private float threshold, corrlation;
            private AnomalyDetectionUtil.Line lin_reg;

            public CorrelatedFeatures(int feature1, int feature2, float corrlation, float threshold, AnomalyDetectionUtil.Line lin_reg)
            {
                this.feature1 = feature1;
                this.feature2 = feature2;
                this.corrlation = corrlation;
                this.threshold = threshold;
                this.lin_reg = lin_reg;
            }
            public int getFeature1()
            {
                return this.feature1;
            }
            public int getFeature2()
            {
                return this.feature2;
            }
            public AnomalyDetectionUtil.Line getLinearReg()
            {
                return this.lin_reg;
            }
            public float getThreshold()
            {
                return this.threshold;
            }
        }

        private static float valueInColumn(int index, string line)
        {
            string[] values = line.Split(",");
            return float.Parse(values[index]);
        }

        private static List<float> getColumn(int index, string[] lines)
        {
            List<float> column = new List<float>();
            int size = lines.Length;
            for (int i = 0; i < size; i++)
            {
                column.Add(valueInColumn(index, lines[i]));
            }
            return column;
        }

        private static List<Tuple<int, int>> maxCorrelated()
        {
            List<Tuple<int, int>> maxCfs = new List<Tuple<int, int>>();
            foreach (CorrelatedFeatures cf in cfList)
            {
                maxCfs.Add(new Tuple<int, int>(cf.getFeature1(), cf.getFeature2()));
            }
            return maxCfs;
        }

        public static List<Tuple<int, int>> learnNormal(string[] lines)
        {
            float maxCorrelation, absoluteCorrelation;
            int indexOfMax = 0;
            List<float> maxColumn = new List<float>();
            List<float> iColumn, jColumn;
            int size = lines[0].Split(",").Length;
            for (int i = 0; i < size - 1; i++)
            {
                maxCorrelation = 0;
                absoluteCorrelation = 0;
                iColumn = getColumn(i, lines);
                for (int j = i + 1; j < size; j++)
                {
                    jColumn = getColumn(j, lines);
                    absoluteCorrelation = Math.Abs(AnomalyDetectionUtil.pearson(iColumn, jColumn));
                    if (absoluteCorrelation > maxCorrelation)
                    {
                        maxCorrelation = absoluteCorrelation;
                        maxColumn = jColumn;
                        indexOfMax = j;
                    }
                }
                //if the correlation is greater or equal than the threshold
                // we are adding the correlated features to the cf vector
                createAndAddCorrelatedFeaturesVector(iColumn, maxColumn, i, indexOfMax, maxCorrelation);
            }
            return maxCorrelated();
        }

        private static void createAndAddCorrelatedFeaturesVector(List<float> currentColumn, List<float> maxColumn, int currentFeatureIndex, int maxFeatureIndex, float maxCorrelation)
        {
            if (maxCorrelation >= threshold)
            {
                List<AnomalyDetectionUtil.Point> pointsList = new List<AnomalyDetectionUtil.Point>();
                int size = currentColumn.Count();
                for (int i = 0; i < size; i++)
                    pointsList.Add(new AnomalyDetectionUtil.Point(currentColumn[i], maxColumn[i]));
                AnomalyDetectionUtil.Line linearReg = AnomalyDetectionUtil.linear_reg(pointsList);
                // we find the threshold
                float threshold = calculate_threshold(pointsList, linearReg);
                CorrelatedFeatures cFeatures = new CorrelatedFeatures(currentFeatureIndex, maxFeatureIndex, maxCorrelation,
                    (float)1.1 * threshold, linearReg);
                cfList.Add(cFeatures);
            }
        }

        private static float calculate_threshold(List<AnomalyDetectionUtil.Point> pointsList, AnomalyDetectionUtil.Line linearReg)
        {
            float maxDev = 0, currentDev;
            int size = pointsList.Count();
            for (int k = 0; k < size; k++)
            {
                currentDev = AnomalyDetectionUtil.dev(pointsList[k], linearReg);
                if (currentDev > maxDev)
                {
                    maxDev = currentDev;
                }
            }
            return maxDev;
        }

        private static List<AnomalyDetectionUtil.Point> makePointsList(List<float> column1, List<float> column2)
        {
            int size = column1.Count();
            List<AnomalyDetectionUtil.Point> pointList = new List<AnomalyDetectionUtil.Point>();
            for (int i = 0; i < size; i++)
            {
                pointList.Add(new AnomalyDetectionUtil.Point(column1[i], column2[i]));
            }
            return pointList;
        }

        private static List<ScatterPoint> makeScatterPoints(List<float> column1, List<float> column2)
        {
            int size = column1.Count();
            List<ScatterPoint> pointList = new List<ScatterPoint>();
            for (int i = 0; i < size; i++)
            {
                pointList.Add(new ScatterPoint(column1[i], column2[i]));
            }
            return pointList;
        }

        //Dictionary: Tuple: int 1 is feature 1, int 2 is feature 2. long is timestep.
        public static Dictionary<Tuple<float, float>, long> detect(string[] lines)
        {
            detectResult = new Dictionary<Tuple<float, float>, long>();
            List<AnomalyDetectionUtil.Point> pointList;
            int sizeOfCF = cfList.Count(), sizeOfPointList;
            CorrelatedFeatures currFeature;
            for (int i = 0; i < sizeOfCF; i++)
            {
                pointList = makePointsList(getColumn(cfList[i].getFeature1(), lines), getColumn(cfList[i].getFeature2(), lines));
                sizeOfPointList = pointList.Count();
                currFeature = cfList[i];
                for (int j = 0; j < sizeOfPointList; j++)
                {
                    if (AnomalyDetectionUtil.dev(pointList[j], currFeature.getLinearReg()) > currFeature.getThreshold())
                    {
                        detectResult.Add(new Tuple<float, float>(currFeature.getFeature1(), currFeature.getFeature2()), j + 1);
                    }
                }
            }
            return detectResult;
        }

        public static List<float> returnDetails(int proprety)
        {
            foreach (CorrelatedFeatures cf in cfList)
            {
                if (cf.getFeature1() == proprety)
                {
                    List<float> l = new List<float>();
                    l.Add(cf.getLinearReg().getA());
                    l.Add(cf.getLinearReg().getB());
                    return l;
                }
            }
            return new List<float>();
        }

        public static Func<double, double> Equation { get; set; }


        //public static OxyPlot.PlotModel returnShapeAnnotation(string[] lines, int feature, int corralated)
        //{
        //    List<ScatterPoint> greyPoints = makeScatterPoints(getColumn(feature, lines), getColumn(corralated, lines));
        //    List<ScatterPoint> redPoints = makeRedPoints();
        //    OxyPlot.Annotations.FunctionAnnotation line = new OxyPlot.Annotations.FunctionAnnotation();
        //    List<float> lineDetails = new List<float>();
        //    if (lineDetails != null)
        //        line.Equation = (x) => lineDetails[0] * x + lineDetails[1];
        //    var s1 = new ScatterSeries();
        //    int size = greyPoints.Count();
        //    for (int i = 0; i < size; i++)
        //    {
        //        s1.Points.Add(greyPoints[i]);
        //    }

        //    var s2 = new ScatterSeries()
        //    {
        //        MarkerType = MarkerType.Diamond,
        //        MarkerStrokeThickness = 0,
        //        MarkerFill = OxyColor.FromRgb(255,0,0)    
        //    };
        //    size = redPoints.Count();
        //    for (int i = 0; i < size; i++)
        //    {
        //        s2.Points.Add(redPoints[i]);
        //    }

        //    OxyPlot.PlotModel p = new OxyPlot.PlotModel();
        //    p.Series.Add(s1);
        //    p.Series.Add(s2);
        //    p.Annotations.Add(line);

        //    return p;
        //}


        public static Annotation drawGraph(string[] lines, int feature, int corralated)
        {
            List<float> lineDetails = returnDetails(feature);
            FunctionAnnotation line = new FunctionAnnotation();
            if (lineDetails.Count > 0)
            {

                line.Equation = (x) => lineDetails[0] * x + lineDetails[1];
                if (line.Equation == null)
                {
                    return null;
                }
            }
            return line;
        }
    }
}
