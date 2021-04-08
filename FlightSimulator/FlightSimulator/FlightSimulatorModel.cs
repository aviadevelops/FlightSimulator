using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool isPaused = true;
        private bool isDone = false;
        public event PropertyChangedEventHandler PropertyChanged;
        private float rudder = 0;
        private float throttle = 0;
        private string[] lines;
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
        private int displayIndex = 0;
        private string playBackPath;
        public float BigEllipseCanvasWidth;
        public float BigEllipseCanvasHeight;
        public float LittleEllipseCanvasWidth;
        public float LittleEllipseCanvasHeight;
        public string graphName = "Graph";
        private Dictionary<string, int> statIndecies = new Dictionary<string, int>();
        private List<DataPoint> points = new List<DataPoint>();
        private List<string> listBox = new List<string>();

        public List<string> ListBox
        {
            get { return this.listBox; }
            set { notifyPropertyChanged("ListBox"); }
        }


        public IList<DataPoint> Points
        {
            get { return this.points; }
            set {
                this.points = new List<DataPoint>(value);
                notifyPropertyChanged("Points"); 
            }
        }

        public string GraphName
        {
            get { return this.graphName; }
            set {
                this.graphName = value;
                notifyPropertyChanged("GraphName"); 
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
            GraphName = listBox[0];
        }
        public string[] getLines()
        {
            return this.lines;
        }

        public void setLines(string[] newLines)
        {
            this.lines = newLines;
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


        public float valueInColums(int index, string line)
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
                x = this.valueInColums(index, line);
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
                x = this.valueInColums(index, line);
                if (min > x)
                {
                    min = x;
                }
            }
            return min;
        }


        public void initIndex()
        {
            this.rudderIndex = getIndex("rudder");
            this.throttleIndex = getIndex("throttle");
            this.aileronIndex = getIndex("aileron");
            this.elevatorIndex = getIndex("elevator");


            statIndecies["altimeter"] = getIndex("altimeter_indicated-altitude-ft");
            statIndecies["airspeed"] = getIndex("airspeed-kt");
            statIndecies["heading_deg"] = getIndex("heading-deg");
            statIndecies["pitch"] = getIndex("pitch-deg");
            statIndecies["roll"] = getIndex("roll-deg");
            statIndecies["yaw"] = getIndex("side-slip-deg");
        }


        public void sendInLoop(string[] lines, int max)
        {
            string msg;
            isDone = false;
            string s = "reg-flight.csv";
            char[] charArr = s.ToCharArray();
            f(charArr, "");
            while (true)
            {
                for (; CurrentTimeStep <= max; CurrentTimeStep++)
                {
                    CurrentTimeSpan = TimeSpan.FromSeconds(CurrentTimeStep / 10);

                    if (isPaused || isDone)
                        break;

                    if (CurrentTimeStep % 10 == 0)
                        setCurrentPoint();

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


        public int getIndex(string value)
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
            Rudder = this.valueInColums(this.rudderIndex, line);
            Throttle = this.valueInColums(this.throttleIndex, line);
            Aileron = this.valueInColums(this.aileronIndex, line);
            Elevator = this.valueInColums(this.elevatorIndex, line);
            Altimeter = valueInColums(statIndecies["altimeter"], line);
            Airspeed = valueInColums(statIndecies["airspeed"], line);
            Heading_deg = valueInColums(statIndecies["heading_deg"], line);
            Pitch = valueInColums(statIndecies["pitch"], line);
            Roll = valueInColums(statIndecies["roll"], line);
            Yaw = valueInColums(statIndecies["yaw"], line);
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
                    this.listBox.Add(node["name"].InnerText + "-" + this.countInstances(node["name"].InnerText));
                } else
                {
                    this.listBox.Add(node["name"].InnerText);
                }

            }
        }

        private int countInstances(string value)
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

        private int findLine(string line)
        {
            return getIndex(line);
        }

        public void setDisplayIndex(string line)
        {
            
            Points = new List<DataPoint>();
            GraphName = line;
            if (countInstances(line) > 1) {
                int count;
                for (count = 0; count < ListBox.Count; count++)
                {
                    if (ListBox[count] == line)
                        break;
                }
                this.displayIndex = count;
            }
            else
                this.displayIndex = findLine(line);
        }

        //private static List<DataPoint> temporaryPoints = new List<DataPoint>();

        public void setCurrentPoint()
        {
            List<DataPoint> tmp = new List<DataPoint>();
            for (int i = 0; i <= currentTimeStep; i++)
                tmp.Add(new DataPoint((Double)i, (Double)valueInColums(displayIndex, lines[i])));
            Points = tmp;
        }

        public string f(char[] csvFileName, string str)
        {

            IntPtr ts = UploadDll.CreateTimeSeriesFromCsv(csvFileName);
            IntPtr anomalyDetector = UploadDll.CreateSimpleAnomalyDetector();
            IntPtr sw = UploadDll.CreatestringWrapper();

            sw = UploadDll.getCorrelatedFeature(anomalyDetector, sw);
            string s = "";
            for (int i = 0; i < UploadDll.Length(sw); i++)
            {
                s += UploadDll.GetChar(sw, i);
            }
            return s;

        }
    }

   
}
