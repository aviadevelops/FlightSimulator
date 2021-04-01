using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class FlightSimulatorModel : INotifyPropertyChanged
    {
        private TcpClient client;
        private NetworkStream stream;
        private int sleepFor = 100;
        private int currentTimeStep = 0;
        private TimeSpan currentTimeSpan;
        private bool isPaused = true;
        private bool isDone = false;
        public event PropertyChangedEventHandler PropertyChanged;

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

        public void sendInLoop(string[] lines, int max)
        {
            string msg;
            isDone = false;
            while (true)
            {
                for (; CurrentTimeStep <= max; CurrentTimeStep++)
                {
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


    }
}
