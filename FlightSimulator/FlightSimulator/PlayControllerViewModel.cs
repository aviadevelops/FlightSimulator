using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace FlightSimulator 
{
    public class PlayControllerViewModel : INotifyPropertyChanged
    {
    bool hasStarted = false;
    private FlightSimulatorModel fsmodel;
    public event PropertyChangedEventHandler PropertyChanged;

        public int VM_CurrentTimeStep
        {
            get { return fsmodel.CurrentTimeStep; }
            set { 
                fsmodel.CurrentTimeStep = value;
                  }
        }

        public TimeSpan VM_CurrentTimeSpan
        {
            get { return fsmodel.CurrentTimeSpan; }
            set { fsmodel.CurrentTimeSpan = value; }    
        } 

        public PlayControllerViewModel(FlightSimulatorModel fsmodel)
        {
            this.fsmodel = fsmodel;
            fsmodel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) 
            {
                notifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void notifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int get_csv_length()
        {
            if (this.fsmodel.getLines() == null)
            {
                return 0;
            }
            return this.fsmodel.getLines().Length;
        }

       public string[] load_csv()
        {
            string path = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            if (path == null)
            {
                string text = "Please choose a path";
                MessageBox.Show(text);
                return null;
            }
            hasStarted = false;
            fsmodel.SetIsDone(true);
            this.fsmodel.setLines(File.ReadAllLines(path));
            VM_CurrentTimeStep = 0;
            return this.fsmodel.getLines(); // return lines[]
        }

        public void play()
        {
            if (this.fsmodel.getLines() == null)
            {
                return;
            }
            fsmodel.SetIsPaused(false);
            int max = get_csv_length() - 1;
            if (VM_CurrentTimeStep == max)
            {
                VM_CurrentTimeStep = 0;
                return;
            }
            
            if (!hasStarted)
            {
                hasStarted = true;
                var thread = new Thread(() => fsmodel.sendInLoop(this.fsmodel.getLines(), max));
                thread.Start();
            }

        }

        public void pause()
        {
            fsmodel.SetIsPaused(true);
        }

        public void changeSpeed(string text)
        {
            try
            {
                double temp = Convert.ToDouble(text);
                if (temp <= 0)
                    throw new Exception();
                temp = 100.0 / temp;
                fsmodel.SetSleepFor((int)temp);
            }
            catch (Exception) {; }
        }

        public void close()
        {
            fsmodel.terminateConnection();
        }

        public void sendPlayBackPathToModel(string path)
        {
            this.fsmodel.setPlayBackPath(path);
        }

        public void displayGraph(string clicked)
        {
            fsmodel.setDisplayIndex(clicked); 
        }
    }
}
