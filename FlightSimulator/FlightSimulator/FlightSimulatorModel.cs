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

namespace FlightSimulator
{
    public class FlightSimulatorModel : INotifyPropertyChanged
    {
        private TcpClient client;
        private NetworkStream stream;
        private int sleepFor = 100;
        private int currentTimeStep = 0;
        private TimeSpan currentTimeSpan;
        private SimpleAnomalyDetector anomalyDetector;
        private bool isPaused = true;
        private bool isDone = false;
        private bool displayPoints = false;
        public event PropertyChangedEventHandler PropertyChanged;
        private float rudder = 0;
        private float throttle = 0;
        private string[] testLines;
        private string[] trainLines;
        private int rudderIndex = 0;
        private int throttleIndex = 0;
        private float altimeter = 0;
        private float airspeed = 0;
        private float heading_deg = 0;
        private float pitch = 0;
        private float roll = 0;
        private float yaw = 0;
        private float aileron;
        private float elevator;
        private int aileronIndex = 0;
        private int elevatorIndex = 0;
        private int[] displayIndexes = { 0, 0, 0 };
        private string trainCSVFile = "";
        private string testCSVFile = "";
        private string playBackPath;
        private string graphNameLeft = "Graph";
        private string graphNameRight = "Graph";
        private string graphNameBottom = "Graph";
        public float BigEllipseCanvasWidth;
        public float BigEllipseCanvasHeight;
        public float LittleEllipseCanvasWidth;
        public float LittleEllipseCanvasHeight;
        private Dictionary<string, int> statIndecies = new Dictionary<string, int>();
        private List<DataPoint> pointsLeft = new List<DataPoint>();
        private List<DataPoint> pointsRight = new List<DataPoint>();
        private List<DataPoint> pointsBottom = new List<DataPoint>();
        private List<DataPoint> pointsBottomLine = new List<DataPoint>();
        private List<string> listBox = new List<string>();

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

        public IList<DataPoint> PointsBottomLine
        {
            get { return this.pointsBottomLine; }
            set
            {
                this.pointsBottomLine = new List<DataPoint>(value);
                notifyPropertyChanged("PointsBottomLine");
            }
        }

        public IList<DataPoint> PointsBottom
        {
            get { return this.pointsBottom; }
            set
            {
                this.pointsBottom = new List<DataPoint>(value);
                notifyPropertyChanged("PointsBottom");
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

        public bool loadCsv(string path, bool isTrain)
        {
            try
            {
                if (isTrain)
                {
                    trainCSVFile = path;
                    trainLines = File.ReadAllLines(path);
                    train();

                }
                else
                {
                    if (trainCSVFile == "")
                        return false;
                    isDone = true;
                    testLines = File.ReadAllLines(path);
                    CurrentTimeStep = 0;
                    initIndex();
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
        public string[] getLines(int num)
        {
            if (num == 1)
                return this.trainLines; 
            return this.testLines;
        }

        public void setLines(string[] newLines, int num)
        {
            if (num == 1)
                this.trainLines = newLines;
            else
                this.testLines = newLines;
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
                for (; CurrentTimeStep <= max; CurrentTimeStep++)
                {
                    CurrentTimeSpan = TimeSpan.FromSeconds(CurrentTimeStep / 10);

                    if (isPaused || isDone)
                        break;

                    if (CurrentTimeStep % 10 == 0)
                    {
                        setCurrentPoint(0);
                        setCurrentPoint(1);
                        if (displayPoints)
                            displayBottomPoints();
                    }
                        
                    changeStats(lines[currentTimeStep]);

                    msg = lines[CurrentTimeStep] + "\n";
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    stream.Write(sendData, 0, sendData.Length);
                    Console.WriteLine(msg);
                    Thread.Sleep(sleepFor);
                }
                if (isDone)
                    break;
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

        public void setGraphDisplayIndex(string line)
        {
            PointsLeft = new List<DataPoint>();
            GraphNameLeft = line;
            this.displayIndexes[0] = getIndexFromListBox(line);
            if (trainLines != null)
            {
                findMaxCorrelatedFeature(line);
                displayBottomGraph(line);
            }  
        }

        public void findMaxCorrelatedFeature(string property)
        {
            PointsRight = new List<DataPoint>();
            string maxCorralated = anomalyDetector.findMaxCorralatedFeature(property);
            if (maxCorralated == "CF Unavailable")
            {
                PointsRight = new List<DataPoint>();
                return;
            }
            GraphNameRight = maxCorralated;
            this.displayIndexes[1] = getIndexFromListBox(maxCorralated);
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

        public List<float> getColumn(int index)
        {
            List<float> column = new List<float>();
            for (int i = 0; i < trainLines.Length; i++)
            {
                column.Add(valueInColumn(index, trainLines[i]));
            }
            return column;
        }


        public void train()
        {
            anomalyDetector = new SimpleAnomalyDetector(this, (float)0.5);
            anomalyDetector.learnNormal();
        }


        public void displayBottomPoints() 
        {
            List<DataPoint> Points = new List<DataPoint>();
            float x, y;
            for (int i = currentTimeStep; i >= 0 && i > currentTimeStep - 300; i--)
            {
                x = valueInColumn(displayIndexes[0], testLines[i]);
                y = valueInColumn(displayIndexes[1], testLines[i]);
                Points.Add(new DataPoint((Double)x, (Double)y));
            }
            PointsBottom = Points;
        }

        public void displayBottomGraph(string proprety)
        {
            List<float> data = anomalyDetector.returnLineDetails(proprety);
            AnomalyDetectionUtil.Line linearReg;
            List<DataPoint> Points = new List<DataPoint>();
            try
            {
                linearReg = new AnomalyDetectionUtil.Line(data[0], data[1]);
                GraphNameBottom = "Linear Regression";
            } 
            catch (Exception)
            {
                GraphNameBottom = "ERR: Linear Regression Unavailable";
                GraphNameRight = "CF Unavailable";
                PointsBottom = new List<DataPoint>();
                PointsBottomLine = new List<DataPoint>();
                displayPoints = false;
                return;
            }
            // data[0] is a, data[1] is b
            // create graph of linear line with starting time step and last time step
            float startX = findMin(testLines, displayIndexes[0]);
            float startY = findMax(testLines, displayIndexes[0]);
            Points.Add(new DataPoint((Double)startX, (Double)linearReg.f(startX)));
            Points.Add(new DataPoint((Double)startY, (Double)linearReg.f(startY)));
            PointsBottomLine = Points;
            displayPoints = true;
        }
    }
}
