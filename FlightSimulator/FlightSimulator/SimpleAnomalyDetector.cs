using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    public class SimpleAnomalyDetector
    {
        private FlightSimulatorModel fsmodel;
        private AnomalyDetectionUtil calcUtil = new AnomalyDetectionUtil();
        private float threshold = (float)0.9;
        private List<CorrelatedFeatures> cfList;

        public class CorrelatedFeatures
        {
            private string feature1, feature2;  // names of the correlated features
            private float corrlation, threshold, centerX, centerY;
            private AnomalyDetectionUtil.Line lin_reg;

            public CorrelatedFeatures(string feature1, string feature2,
                float corrlation, float threshold, float centerX, float centerY,
                AnomalyDetectionUtil.Line lin_reg)
            {
                this.feature1 = feature1;
                this.feature2 = feature2;
                this.corrlation = corrlation;
                this.threshold = threshold;
                this.centerX = centerX;
                this.centerY = centerY;
                this.lin_reg = lin_reg;
            }
            public string getFeature1()
            {
                return this.feature1;
            }
            public string getFeature2()
            {
                return this.feature2;
            }
            public AnomalyDetectionUtil.Line getLinearReg() {
                return this.lin_reg;
            }
        }
        public SimpleAnomalyDetector(FlightSimulatorModel fsmodel, float threshold)
        {
            this.fsmodel = fsmodel;
            this.threshold = threshold;
            this.cfList = new List<CorrelatedFeatures>();
        }

        public void learnNormal()
        {
            float maxCorrelation, absoluteCorrelation;
            string nameOfMax = "";
            List<float> maxColumn = new List<float>();
            List<float> iColumn, jColumn;
            int size = fsmodel.ListBox.Count() - 1;
            for (int i = 0; i < size; i++)
            {
                maxCorrelation = 0;
                absoluteCorrelation = 0;
                iColumn = fsmodel.getColumn(i);
                for (int j = i + 1; j < fsmodel.ListBox.Count(); j++)
                {
                    jColumn = fsmodel.getColumn(j);
                    absoluteCorrelation = Math.Abs(calcUtil.pearson(iColumn, jColumn));
                    if (absoluteCorrelation > maxCorrelation)
                    {
                        maxCorrelation = absoluteCorrelation;
                        maxColumn = jColumn;
                        nameOfMax = fsmodel.ListBox[j];
                    }
                }
                //if the correlation is greater or equal than the threshold
                // we are adding the correlated features to the cf vector
                createAndAddCorrelatedFeaturesVector(iColumn, maxColumn, fsmodel.ListBox[i], nameOfMax, maxCorrelation);
            }
        }

        protected void createAndAddCorrelatedFeaturesVector(List<float> currentColumn, List<float> maxColumn, string currentFeatureName, string maxFeatureName, float maxCorrelation)
        {
            if (maxCorrelation >= this.threshold)
            {
                List<AnomalyDetectionUtil.Point> pointsList = new List<AnomalyDetectionUtil.Point>();
                int size = currentColumn.Count();
                for (int i = 0; i < size; i++)
                    pointsList.Add(new AnomalyDetectionUtil.Point(currentColumn[i], maxColumn[i]));
                AnomalyDetectionUtil.Line linearReg = calcUtil.linear_reg(pointsList);
                // we find the threshold
                float threshold = calculate_threshold(pointsList, linearReg);
                CorrelatedFeatures cFeatures = new CorrelatedFeatures(currentFeatureName, maxFeatureName, maxCorrelation,
                    (float)1.1 * threshold, 0, 0, linearReg);
                this.cfList.Add(cFeatures);
            }
        }

        protected float calculate_threshold(List<AnomalyDetectionUtil.Point> pointsList, AnomalyDetectionUtil.Line linearReg)
        {
            float maxDev = 0, currentDev;
            int size = pointsList.Count();
            for (int k = 0; k < size; k++)
            {
                currentDev = calcUtil.dev(pointsList[k], linearReg);
                if (currentDev > maxDev)
                {
                    maxDev = currentDev;
                }
            }
            return maxDev;
        }

        public string findMaxCorralatedFeature(string property)
        {
            foreach (CorrelatedFeatures cf in this.cfList) {
                if (cf.getFeature1() == property)
                    return cf.getFeature2();
            }
            return "CF Unavailable";
        }

        public List<float> returnLineDetails(string proprety) {
            foreach (CorrelatedFeatures cf in this.cfList)
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

    }
}
