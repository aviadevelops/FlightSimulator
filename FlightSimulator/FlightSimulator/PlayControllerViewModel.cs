using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator 
{
    class PlayControllerViewModel : INotifyPropertyChanged
    {
    private string[] lines;
    bool hasStarted = false;
    private FlightSimulatorModel fsmodel;
    public event PropertyChangedEventHandler PropertyChanged;

    public int VM_CurrentTimeStep
        {
            get { return fsmodel.CurrentTimeStep; }
            set { fsmodel.CurrentTimeStep = value;
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
            fsmodel.connect();
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
            if (lines == null)
            {
                return 0;
            }
            return lines.Length;
        }

       public void load_csv()
        {
            string path = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            if (path == null)
            {
                string text = "Please choose a path";
                MessageBox.Show(text);
                return;
            }
            hasStarted = false;
            fsmodel.SetIsDone(true);
            lines = File.ReadAllLines(path);
            VM_CurrentTimeStep = 0;
        }

        public void play()
        {
            if (lines == null)
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
                var thread = new Thread(() => fsmodel.sendInLoop(lines, max));
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
    }
}
