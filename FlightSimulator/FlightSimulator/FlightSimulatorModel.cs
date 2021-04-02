using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private string playBackPath;



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
        }
        public string[] getLines()
        {
            return this.lines;
        }

        public void setLines(string [] newLines)
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

        public TimeSpan CurrentTimeSpan
        {
            get { return currentTimeSpan; }
            set { 
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
            string [] values = line.Split(",");
            return float.Parse(values[index]);
        }

        public float findMax(string [] lines, int index)
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
            this.rudderIndex = this.getIndex("rudder");
            this.throttleIndex = this.getIndex("throttle");
        }


        public void sendInLoop(string[] lines, int max)
        {
            string msg;
            isDone = false;

            while (true)
            {
                for (; CurrentTimeStep <= max; CurrentTimeStep++)
                {
                    Rudder = this.valueInColums(this.rudderIndex, lines[CurrentTimeStep]);
                    Throttle = this.valueInColums(this.throttleIndex, lines[CurrentTimeStep]);
                    CurrentTimeSpan = TimeSpan.FromSeconds(CurrentTimeStep/10);
                    if (isPaused || isDone)
                        break;
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
                        } else
                        {
                            counter++;
                        }
                        
                    }
                    
                   
                }
         
            }
            return counter;
        }


    }
}
