using AnomalyDetectorUtil;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;



/*
 * FlightSimulatorModel is the main model of the project. In this class we create a tcp client with the connect() method
 * and send data to the FlightGear in sendInLoop() method. In sendInLoop() we send the csv lines at certain intervals to the FG.
 * We also have here, a train() which responsible for calling the learn normal method (from the dll), and 
 * we have the test() method which responsible for calling the detect method (from the dll)
 * we store the properties of the project here and for each property call the notifyPropertyChanged method. 
 * we also have here some another functions that help us to parse the xml files, and load the dll.
 * The property pointsLeft is used for the graph of the feature the we want to research, and pointsRight is for his most 
 * CorralatedFreatures. pointsBottomGray is for the graph below - that represents the points of the last 30 seconds of the flight.
 * pointsBottomRed represents the anomaly detection points.
 */
namespace FlightSimulator
{
    public class FlightSimulatorModel : INotifyPropertyChanged
    {
        private int sleepFor = 100;
        private int currentTimeStep = 0;
        private bool isPaused = true;
        private bool isDone = false;
        private bool displayPoints = false;
        public event PropertyChangedEventHandler PropertyChanged;
        private string[] trainLines, testLines;
        private float currentMinX, currentMinY, currentMaxX, currentMaxY;
        private int aileronIndex = 0, elevatorIndex = 0, rudderIndex = 0, throttleIndex = 0;
        private float altimeter = 0, aileron = 0, airspeed = 0, elevator = 0, heading_deg = 0, pitch = 0,
            roll = 0, rudder = 0, throttle = 0, yaw = 0;
        private int maxSpeed = 0, minSpeed = 0, minDeg = 0, maxDeg=0, minYaw=0, maxYaw=0, minPitch=0, maxPitch=0, maxRoll=0,minRoll=0, minAlt=0,maxAlt=0;
        private int[] displayIndexes = { 0, 0};
        private string trainCSVFile = "", testCSVFile = "";
        private string playBackPath;
        private string graphNameLeft = "Feature";
        private string graphNameRight = "Correlated Feature";
        private string graphNameBottom = "Inspection View";
        public float BigEllipseCanvasWidth;
        public float BigEllipseCanvasHeight;
        public float LittleEllipseCanvasWidth;
        public float LittleEllipseCanvasHeight;

        public void SetGaugesMinMax()
        {
            if (testLines == null)
            {
                return;
            }
            MaxAlt = (int)this.findMax(testLines,statIndecies["altimeter"]);
            MinAlt = (int)this.findMin(testLines, statIndecies["altimeter"]);

            MaxSpeed = (int)this.findMax(testLines, statIndecies["airspeed"]);
            MinSpeed = (int)this.findMin(testLines, statIndecies["airspeed"]);

            MaxDeg = (int)this.findMax(testLines, statIndecies["heading_deg"]);
            MinDeg = (int)this.findMin(testLines, statIndecies["heading_deg"]);

            MaxPitch = (int)this.findMax(testLines, statIndecies["pitch"]);
            MinPitch = (int)this.findMin(testLines, statIndecies["pitch"]);

            MaxRoll = (int)this.findMax(testLines, statIndecies["roll"]);
            MinRoll = (int)this.findMin(testLines, statIndecies["roll"]);

            MaxYaw = (int)this.findMax(testLines, statIndecies["yaw"]);
            MinYaw = (int)this.findMin(testLines, statIndecies["yaw"]);
        }

        private TcpClient client;
        private NetworkStream stream;
        private TimeSpan currentTimeSpan;
        private List<string> listBox = new List<string>();
        private List<DataPoint> pointsLeft = new List<DataPoint>();
        private List<DataPoint> pointsRight = new List<DataPoint>();
        private List<DataPoint> pointsBottomGray = new List<DataPoint>();
        private List<DataPoint> pointsBottomRed = new List<DataPoint>();
        private List<TimeSpan> currentErrors = new List<TimeSpan>();
        private List<Tuple<int, int>> maxCorralatedFreatures = new List<Tuple<int, int>>();
        private Dictionary<string, int> statIndecies = new Dictionary<string, int>();
        private Dictionary<int, List<int>> errorTimes = new Dictionary<int, List<int>>();
        private Dictionary<int, List<DataPoint>> savedPoints = new Dictionary<int, List<DataPoint>>();
        private Object dll;

        public float CurrentMaxX
        {
            get { return this.currentMaxX; }
            set
            {
                this.currentMaxX = value;
                notifyPropertyChanged("CurrentMaxX");
            }
        }

        public int MaxSpeed
        {
            get { return this.maxSpeed; }
            set
            {
                this.maxSpeed = value;
                notifyPropertyChanged("MaxSpeed");
            }
        }

        public int MinSpeed
        {
            get { return this.minSpeed; }
            set
            {
                this.minSpeed = value;
                notifyPropertyChanged("MinSpeed");
            }
        }



        public int MaxYaw
        {
            get { return this.maxYaw; }
            set
            {
                this.maxYaw = value;
                notifyPropertyChanged("MaxYaw");
            }
        }

        public int MinYaw
        {
            get { return this.minYaw; }
            set
            {
                this.minYaw = value;
                notifyPropertyChanged("MinYaw");
            }
        }

        public int MaxRoll
        {
            get { return this.maxRoll; }
            set
            {
                this.maxRoll = value;
                notifyPropertyChanged("MaxRoll");
            }
        }

        public int MinRoll
        {
            get { return this.minRoll; }
            set
            {
                this.minRoll = value;
                notifyPropertyChanged("MinRoll");
            }
        }

        public int MaxPitch
        {
            get { return this.maxPitch; }
            set
            {
                this.maxPitch = value;
                notifyPropertyChanged("MaxPitch");
            }
        }

        public int MinPitch
        {
            get { return this.minPitch; }
            set
            {
                this.minPitch = value;
                notifyPropertyChanged("MinPitch");
            }
        }


        public int MaxAlt
        {
            get { return this.maxAlt; }
            set
            {
                this.maxAlt = value;
                notifyPropertyChanged("MaxAlt");
            }
        }

        public int MinAlt
        {
            get { return this.minAlt; }
            set
            {
                this.minAlt = value;
                notifyPropertyChanged("MinAlt");
            }
        }


        public int MaxDeg
        {
            get { return this.maxDeg; }
            set
            {
                this.maxDeg = value;
                notifyPropertyChanged("MaxDeg");
            }
        }

        public int MinDeg
        {
            get { return this.minDeg; }
            set
            {
                this.minDeg = value;
                notifyPropertyChanged("MinDeg");
            }
        }




        public float CurrentMinX
        {
            get { return this.currentMinX; }
            set
            {
                this.currentMinX = value;
                notifyPropertyChanged("CurrentMinX");
            }
        }

        public float CurrentMaxY
        {
            get { return this.currentMaxY; }
            set
            {
                this.currentMaxY = value;
                notifyPropertyChanged("CurrentMaxY");
            }
        }

        public float CurrentMinY
        {
            get { return this.currentMinY; }
            set
            {
                this.currentMinY = value;
                notifyPropertyChanged("CurrentMinY");
            }
        }

        public void updateAxes()
        {
            float minX = this.findMin(this.testLines, this.displayIndexes[0]), minY = this.findMin(this.testLines, this.displayIndexes[1]), maxX = this.findMax(this.testLines, this.displayIndexes[0]), maxY = this.findMax(this.testLines, this.displayIndexes[1]);
            float marginX = (float)((maxX - minX) * 0.75), marginY = (float)((maxY - minY) * 0.75);
            this.CurrentMaxX = this.findMax(this.testLines,this.displayIndexes[0]) + marginX;
            this.CurrentMinX = this.findMin(this.testLines, this.displayIndexes[0]) - marginX;
            this.CurrentMaxY = this.findMax(this.testLines, this.displayIndexes[1]) + marginY;
            this.CurrentMinY = this.findMin(this.testLines, this.displayIndexes[1]) - marginY;

        }


        public List<string> ListBox
        {
            get { return this.listBox; }
            set { notifyPropertyChanged("ListBox"); }
        }


        public IList<DataPoint> PointsLeft
        {
            get { return this.pointsLeft; }
            set {
                this.pointsLeft = new List<DataPoint>(value);
                notifyPropertyChanged("PointsLeft"); 
            }
        }

        public IList<DataPoint> PointsRight
        {
            get { return this.pointsRight; }
            set
            {
                this.pointsRight = new List<DataPoint>(value);
                notifyPropertyChanged("PointsRight");
            }
        }

        public void load_dll(string path)
        {
            if (path == null)
            {
                return;
            }
            string filenameOld = Path.GetFileName(path);
            string filename = filenameOld.Split(".")[0];
            filename = "FlightSimulator." + filename;
            var dllFile = new System.IO.FileInfo(path);
            System.Reflection.Assembly dllAssembly = System.Reflection.Assembly.LoadFile(dllFile.FullName);
            Object dllInstance = (Object)dllAssembly.CreateInstance(filename);
            dll = dllInstance;
            tryTrain();
            //tryTest();
        }

        public void tryTrain()
        {
            if (dll!=null && trainLines != null)
            {
                train();
            }
        }

        public void tryTest()
        {
            if (dll != null && testLines != null)
            {
                test();
            }
        }

        public IList<DataPoint> PointsBottomGray
        {
            get { return this.pointsBottomGray; }
            set
            {
                this.pointsBottomGray = new List<DataPoint>(value);
                notifyPropertyChanged("pointsBottomGray");
            }
        }

        public IList<DataPoint> PointsBottomRed
        {
            get { return this.pointsBottomRed; }
            set
            {
                this.pointsBottomRed = new List<DataPoint>(value);
                notifyPropertyChanged("PointsBottomRed");
            }
        }

        public List<TimeSpan> ErrorsInGraph
        {
            get { return this.currentErrors; }
            set
            {
                this.currentErrors = new List<TimeSpan>(value);
                notifyPropertyChanged("ErrorsInGraph");
            }
        }

        public string GraphNameLeft
        {
            get { return this.graphNameLeft; }
            set {
                this.graphNameLeft = value;
                notifyPropertyChanged("GraphNameLeft"); 
            }
        }

        public string GraphNameRight
        {
            get { return this.graphNameRight; }
            set
            {
                this.graphNameRight = value;
                notifyPropertyChanged("GraphNameRight");
            }
        }

        public string GraphNameBottom
        {
            get { return this.graphNameBottom; }
            set
            {
                this.graphNameBottom = value;
                notifyPropertyChanged("GraphNameBottom");
            }
        }

        public OxyPlot.Wpf.Annotation returnShapeAnnotation()
        {
            if (dll == null)
            {
                return null;
            }
            try
            {
                object[] argslearn = new object[] { (object)displayIndexes[0] };
                var s = (OxyPlot.Wpf.Annotation)dll.GetType().GetMethod("returnShape").Invoke(dll, argslearn);

                return s;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool loadCsv(string path, bool isTrain)
        {
            try
            {
                clearPoints();
                if (isTrain)
                {
                    trainCSVFile = path;
                    trainLines = File.ReadAllLines(path);
                    //CurrentTimeStep = 0;
                    tryTrain();
                    //train();

                }
                else
                {
                    //if (trainCSVFile == "")
                    //    return false;
                    isDone = true;
                    testLines = File.ReadAllLines(path);
                    CurrentTimeStep = 0;
                    initIndex();
                    tryTest();
                    //test();
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int getRudderIndex()
        {
            return this.rudderIndex;
        }
        public int getThrottleIndex()
        {
            return this.throttleIndex;
        }

        public void setPlayBackPath(string playBackPath)
        {
            this.playBackPath = playBackPath;
            this.setListBox();
        }
        // 1 train, 2 test
        public string[] getLines(bool isTrain)
        {
            if (isTrain)
                return this.trainLines; 
            return this.testLines;
        }

        public void SetIsPaused(bool s)
        {
            isPaused = s;
        }

        public void SetIsDone(bool s)
        {
            isDone = s;
        }

        public bool GetIsDone()
        {
            return isDone;
        }

        public bool GetIsPaused()
        {
            return isPaused;
        }
        public void SetSleepFor(int s)
        {
            sleepFor = s;
        }

        public int getAileronIndex()
        {
            return this.aileronIndex;
        }
        public int getElevatorIndex()
        {
            return this.elevatorIndex;
        }

        public int CurrentTimeStep
        {
            get { return currentTimeStep; }
            set
            {
                currentTimeStep = value;
                notifyPropertyChanged("CurrentTimeStep");
            }
        }


        public float Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                notifyPropertyChanged("Rudder");
            }
        }


        public float Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                notifyPropertyChanged("Throttle");
            }
        }

        public float Aileron
        {
            get { return aileron; }
            set
            {
                if (Math.Abs(value * 120) + this.LittleEllipseCanvasWidth / 2 <= this.BigEllipseCanvasWidth / 2)
                {
                    aileron = value * 120;
                    notifyPropertyChanged("Aileron");
                }

            }
        }

        public float Elevator
        {
            get { return elevator; }
            set
            {
                if (Math.Abs(value * 120) + this.LittleEllipseCanvasHeight / 2 <= this.BigEllipseCanvasHeight / 2)
                {
                    elevator = value * 120;
                    notifyPropertyChanged("Elevator");
                }

            }
        }

        public float Altimeter
        {
            get { return altimeter; }
            set
            {
                altimeter = value;
                notifyPropertyChanged("Altimeter");
            }
        }

        public float Airspeed
        {
            get { return airspeed; }
            set
            {
                airspeed = value;
                notifyPropertyChanged("Airspeed");
            }
        }

        public float Heading_deg
        {
            get { return heading_deg; }
            set
            {
                heading_deg = value;
                notifyPropertyChanged("Heading_deg");
            }
        }

        public float Pitch
        {
            get { return pitch; }
            set
            {
                pitch = value;
                notifyPropertyChanged("Pitch");
            }
        }

        public float Roll
        {
            get { return roll; }
            set
            {
                roll = value;
                notifyPropertyChanged("Roll");
            }
        }

        public float Yaw
        {
            get { return yaw; }
            set
            {
                yaw = value;
                notifyPropertyChanged("Yaw");
            }
        }

        public TimeSpan CurrentTimeSpan
        {
            get { return currentTimeSpan; }
            set
            {
                currentTimeSpan = value;
                notifyPropertyChanged("CurrentTimeSpan");
            }
        }


        public void notifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void connect()
        {
        connection:
            try
            {
                client = new TcpClient("localhost", 5400);
                Console.WriteLine("sending data to server...");
                stream = client.GetStream();
            }
            catch (Exception)
            {
                Console.WriteLine("Connection failed");
                goto connection;
            }
        }

        public float valueInColumn(int index, string line)
        {
            string[] values = line.Split(",");
            return float.Parse(values[index]);
        }

        public float findMax(string[] lines, int index)
        {
            float x;
            float max = float.NegativeInfinity;
            foreach (string line in lines)
            {
                x = this.valueInColumn(index, line);
                if (max < x)
                {
                    max = x;
                }
            }
            return max;
        }

        public float findMin(string[] lines, int index)
        {
            float x;
            float min = float.PositiveInfinity;
            foreach (string line in lines)
            {
                x = this.valueInColumn(index, line);
                if (min > x)
                {
                    min = x;
                }
            }
            return min;
        }


        public void initIndex()
        {
            this.rudderIndex = getIndexFromXML("rudder");
            this.throttleIndex = getIndexFromXML("throttle");
            this.aileronIndex = getIndexFromXML("aileron");
            this.elevatorIndex = getIndexFromXML("elevator");


            statIndecies["altimeter"] = getIndexFromXML("altimeter_indicated-altitude-ft");
            statIndecies["airspeed"] = getIndexFromXML("airspeed-kt");
            statIndecies["heading_deg"] = getIndexFromXML("heading-deg");
            statIndecies["pitch"] = getIndexFromXML("pitch-deg");
            statIndecies["roll"] = getIndexFromXML("roll-deg");
            statIndecies["yaw"] = getIndexFromXML("side-slip-deg");
        }


        public void sendInLoop(string[] lines, int max)
        {
            string msg;
            isDone = false;
            setGraphDisplayIndex(ListBox[0]);

  
            while (true)
            {
                if (isPaused || CurrentTimeStep >= max)
                    continue;
                for (; CurrentTimeStep < max; CurrentTimeStep++)
                {

                    CurrentTimeSpan = TimeSpan.FromSeconds(CurrentTimeStep / 10);

                    if (isPaused || isDone)
                        break;

                    if (CurrentTimeStep % 10 == 0)
                    {
                        setCurrentPoint(0);
                        setCurrentPoint(1);
                        if (displayPoints)
                            displayBottomGrayPoints();
                    }
                        
                    changeStats(lines[CurrentTimeStep]);

                   
                    msg = lines[CurrentTimeStep] + "\n";
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    try
                    {
                        stream.Write(sendData, 0, sendData.Length);
                    }
                    catch (Exception) {
                        terminateConnection();
                    }
                
                    //Console.WriteLine(msg);
                    Thread.Sleep(sleepFor);
                }
                if (isDone)
                    break;
                if (currentTimeStep > max)
                    CurrentTimeStep = max;
                displayBottomGrayPoints();
                //CurrentTimeStep++;
            }
        }

        public void terminateConnection()
        {
            isDone = true;
            stream.Close();
            client.Close();            
        }


        public int getIndexFromXML(string value)
        {
            int counter = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(this.playBackPath);
            XmlNodeList nodeList = doc.SelectNodes("/PropertyList/generic/output/chunk");
            foreach (XmlNode node in nodeList)
            {
                // loop over child nodes to get Name and all Number elements
                foreach (XmlNode child in node.ChildNodes)
                {
                    // check node name to decide how to handle the values               
                    if (child.Name.Equals("name"))
                    {
                        if (child.InnerText.Equals(value))
                        {
                            return counter;
                        }
                        else
                        {
                            counter++;
                        }
                    }
                }
            }
            return counter;
        }

        private void changeStats(string line)
        {
            Rudder = this.valueInColumn(this.rudderIndex, line);
            Throttle = this.valueInColumn(this.throttleIndex, line);
            Aileron = this.valueInColumn(this.aileronIndex, line);
            Elevator = this.valueInColumn(this.elevatorIndex, line);
            Altimeter = valueInColumn(statIndecies["altimeter"], line);
            Airspeed = valueInColumn(statIndecies["airspeed"], line);
            Heading_deg = valueInColumn(statIndecies["heading_deg"], line);
            Pitch = valueInColumn(statIndecies["pitch"], line);
            Roll = valueInColumn(statIndecies["roll"], line);
            Yaw = valueInColumn(statIndecies["yaw"], line);
        }


        private void setListBox()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.playBackPath);
            XmlNodeList nodeList = doc.SelectNodes("/PropertyList/generic/output/chunk");
            foreach (XmlNode node in nodeList)
            {
                if (this.listBox.Contains(node["name"].InnerText))
                {
                    this.listBox.Add(node["name"].InnerText + "-" + this.countInstancesInXML(node["name"].InnerText));
                } else
                {
                    this.listBox.Add(node["name"].InnerText);
                }

            }
        }

        private int countInstancesInXML(string value)
        {
            int x = 1;
            for (int i = 0; i < this.listBox.Count(); i++)
            {
                if (this.ListBox[i] == value)
                {
                    x++;
                }
            }
            return x;
        }

        public int getIndexFromListBox(string s)
        {
            int count, size = ListBox.Count();
            for (count = 0; count < size; count++)
            {
                if (ListBox[count] == s)
                    break;
            }
            return count;
        }


        public int getCurrentPropertyIndex() {
            return displayIndexes[0];
        }

        public void setGraphDisplayIndex(string line)
        {
            PointsLeft = new List<DataPoint>();
            GraphNameLeft = line;
            this.displayIndexes[0] = getIndexFromListBox(line);
            if (trainLines != null)
            {
                if (dll == null)
                {
                    return;
                }
                findMaxCorrelatedFeature(line);
                string graphName = (string)dll.GetType().GetMethod("getGraphName").Invoke(dll, null);
                if (GraphNameRight == "CF Unavailable")
                {
                    GraphNameBottom = graphName + " Unavailable";
                    GraphNameRight = "CF Unavailable";
                    PointsBottomGray = new List<DataPoint>();
                    PointsBottomRed = new List<DataPoint>();
                    currentErrors = new List<TimeSpan>();
                    displayPoints = false;
                }
                else {
                    GraphNameBottom = graphName;
                    if(savedPoints.ContainsKey(displayIndexes[0]))
                    {
                        PointsBottomRed = savedPoints[displayIndexes[0]];
                        ErrorsInGraph = setDisplayTimes(displayIndexes[0]);
                    }    
                    displayPoints = true;
                }
                this.updateAxes();
            }  
        }

        public List<TimeSpan> setDisplayTimes(int index)
        {
            List<TimeSpan> displayedTimes = new List<TimeSpan>();
            int size = errorTimes[index].Count();
            for (int i = 0; i < size; i++)
            {
                displayedTimes.Add(TimeSpan.FromMilliseconds(errorTimes[index][i] * 100));
            }
            return displayedTimes;
        }

        public void jumpToTimestep(int timestep)
        {
            CurrentTimeStep = timestep;
            CurrentTimeSpan = TimeSpan.FromSeconds(CurrentTimeStep / 10);
            displayBottomGrayPoints();
        }

        public int findMaxCorrelatedFeature(int property)
        {
            int size = maxCorralatedFreatures.Count();
            for (int i = 0; i < size; i++)
            {
                if (maxCorralatedFreatures[i].Item1 == property)
                    return maxCorralatedFreatures[i].Item2;
            }
            return -1;
        }

        public void findMaxCorrelatedFeature(string property)
        {
            PointsRight = new List<DataPoint>();
            int maxCorrelated = findMaxCorrelatedFeature(getIndexFromListBox(property));
            if (maxCorrelated == -1)
            {
                PointsRight = new List<DataPoint>();
                GraphNameRight = "CF Unavailable";
                return;
            }
            GraphNameRight = ListBox[maxCorrelated];
            this.displayIndexes[1] = getIndexFromListBox(ListBox[maxCorrelated]);
        }

        public void setCurrentPoint(int indicator)
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int i = 0; i <= currentTimeStep; i++)
                tmp.Add(new DataPoint((Double)i, (Double)valueInColumn(displayIndexes[indicator], testLines[i])));
            if (indicator == 0)
                PointsLeft = tmp;
            else if (indicator == 1 && graphNameRight != "CF Unavailable")
                PointsRight = tmp;
        }

        public void setPathToFile(int file, string path)
        {
            if (file == 1)
                this.trainCSVFile = path;
            else if (file == 2)
                this.testCSVFile = path;
        }

        public void train()
        {
            object[] argslearn = new object[] { (object)trainLines };
            maxCorralatedFreatures = (List<Tuple<int, int>>)dll.GetType().GetMethod("learnNormal").Invoke(dll, argslearn);
            //maxCorralatedFreatures = SimpleAnomalyDetector.learnNormal(trainLines);

            tryTest();
        }
        
        public void clearPoints()
        {

        PointsBottomRed = new List<DataPoint>();
        PointsBottomGray = new List<DataPoint>();
            //currentErrors = new List<TimeSpan>();
            //errorTimes = new Dictionary<int, List<int>>();

        }

        public void test()
        {
            object[] argslearn = new object[] { (object)testLines };
            List<Tuple<int, int, long>> tupleDictionary =
                (List<Tuple<int, int, long>>)dll.GetType().GetMethod("detect").Invoke(dll, argslearn);
            int index , cfIndex, timeStep;
            float x, y;
            for (int i = 0; i < ListBox.Count; i++)
            {
                savedPoints[i] = new List<DataPoint>();
                errorTimes[i] = new List<int>();
            }
            foreach (var p in tupleDictionary)
            {
                index = p.Item1;
                cfIndex = p.Item2;
                timeStep = (int)p.Item3;
                x = valueInColumn(index, testLines[timeStep]);
                y = valueInColumn(cfIndex, testLines[timeStep]);
                errorTimes[index].Add(timeStep);
                savedPoints[index].Add(new DataPoint((double)x, (double)y));
            }
        }

        public void displayBottomGrayPoints() 
        {
            List<DataPoint> Points = new List<DataPoint>();
            float x, y;
            for (int i = currentTimeStep; i >= 0 && i > currentTimeStep - 300; i--)
            {
                x = valueInColumn(displayIndexes[0], testLines[i]);
                y = valueInColumn(displayIndexes[1], testLines[i]);
                Points.Add(new DataPoint((double)x, (double)y));
            }
            PointsBottomGray = Points;
        }
    }
}
