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
    private int csvCount = 0;
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
            if (this.fsmodel.getLines(csvCount) == null)
            {
                return 0;
            }
            return this.fsmodel.getLines(csvCount).Length;
        }

        public void load_csv(bool isTrain)
        {
            string path = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            if (path == null)
            {
                string text = "Please choose a path.";
                MessageBox.Show(text);
                return;
            }
            hasStarted = false;
            if (isTrain)
            {
                //MessageBox.Show("Training...");
            }
            bool success = fsmodel.loadCsv(path, isTrain);
            if (isTrain && success)
            {
                MessageBox.Show("Successfuly learned normal attributes. You can now load a flight CSV file to inspect.");
                return;
            }
            if (!isTrain && !success)
            {
                string text = "Please load a learn normal CSV file before you load your flight CSV!";
                MessageBox.Show(text);
                return;
            }
            if (!success)
            {
                string text = "An error has occured, please try again.";
                MessageBox.Show(text);
                return;
            }
            MessageBox.Show("Successfuly loaded flight! Press Play to start.");

        }

        public void play()
        {
            if (this.fsmodel.getLines(csvCount) == null)
            {
                string text = "Please open a non empty CSV";
                MessageBox.Show(text);
                return;
            }
            if (csvCount == 1)
            {
                string text = "Please open a test CSV";
                MessageBox.Show(text);
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
                var thread = new Thread(() => fsmodel.sendInLoop(this.fsmodel.getLines(csvCount), max));
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
            fsmodel.setGraphDisplayIndex(clicked); 
        }
    }
}
