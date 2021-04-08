using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FlightSimulator
{
    class UploadDll
    {
        [DllImport("AnomalyDetector.dll")]
        public static extern IntPtr CreatestringWrapper();
        [DllImport("AnomalyDetector.dll")]
        public static extern int Length(IntPtr s);
        [DllImport("AnomalyDetector.dll")]
        public static extern char GetChar(IntPtr s, int i);
        [DllImport("AnomalyDetector.dll")]
        public static extern void SetString(IntPtr s, IntPtr str);
        [DllImport("AnomalyDetector.dll")]
        public static extern IntPtr CreateTimeSeries();
        [DllImport("AnomalyDetector.dll")]
        public static extern IntPtr CreateTimeSeriesFromCsv(char[] CSVfileName);
        [DllImport("AnomalyDetector.dll")]
        public static extern IntPtr CreateSimpleAnomalyDetector();
        [DllImport("AnomalyDetector.dll")]
        public static extern IntPtr getCorrelatedFeature(IntPtr anomalyDetector, IntPtr s);

    }
}
