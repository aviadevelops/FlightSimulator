using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;


/*
 * PlayControllerViewModel responsible for controlling the play of the flight (User story 2). 
 * we also check that the user load a dll file and after that a train file and finally a test file.
 * As a view model he also need to contain the FlightSimulatorModel, and a view model this class need 
 * to implement INotifyPropertyChanged, and have an event PropertyChangedEventHandler 
 * PropertyChanged, and notifyPropertyChanged function.
 */


namespace FlightSimulator 
{
    public class PlayControllerViewModel : INotifyPropertyChanged
    {
        bool hasStarted = false;
        //bool isDLLLoaded = false;
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

        public float VM_CurrentMaxX
        {
            get { return this.fsmodel.CurrentMaxX; }
            set
            {
                this.fsmodel.CurrentMaxX = value;
            }
        }

        public float VM_CurrentMinX
        {
            get { return this.fsmodel.CurrentMinX; }
            set
            {
                this.fsmodel.CurrentMinX = value;
            }
        }

        public float VM_CurrentMaxY
        {
            get { return this.fsmodel.CurrentMaxY; }
            set
            {
                this.fsmodel.CurrentMaxY = value;
            }
        }



        public float VM_CurrentMinY
        {
            get { return this.fsmodel.CurrentMinY; }
            set
            {
                this.fsmodel.CurrentMinY = value;
            }
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
            if (this.fsmodel.getLines(true) == null && fsmodel.getLines(false) == null)
            {
                return 0;
            }
            else if (fsmodel.getLines(true) == null) return fsmodel.getLines(false).Length;
            else return this.fsmodel.getLines(true).Length;
        }

        public void load_csv(bool isTrain)
        {
            string path = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "csv files (*.csv)|*.csv";
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
            //if (!isTrain && !success)
            //{
            //    MessageBox.Show("Please load a learn normal CSV file before you load your flight CSV!");
            //    return;
            //}
            if (!success)
            {
                MessageBox.Show("An error has occured, please try again.");
                return;
            }
            MessageBox.Show("Successfuly loaded flight! Press Play to start.");

        }

        public void load_dll()
        {
            try
            {
                //isDLLLoaded = false;
                string path = null;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "dll files (*.dll)|*.dll";
                if (openFileDialog.ShowDialog() == true)
                    path = openFileDialog.FileName;
                if (path == null)
                {
                    return;
                }
                this.fsmodel.load_dll(path);
                MessageBox.Show("Loaded dll file.");
            }
            catch (Exception)
            {
                MessageBox.Show("An error has occured. Please try again.");
            }

            //isDLLLoaded = true;
        }

        public void play()
        {
            //if (this.fsmodel.getLines(true) == null)
            //{
            //    MessageBox.Show("Please open a non empty CSV");
            //    return;
            //}
            if (this.fsmodel.getLines(false) == null)
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
                var thread = new Thread(() => fsmodel.sendInLoop(this.fsmodel.getLines(false), max));
                thread.Start();
            }
            

        }

        public OxyPlot.Wpf.Annotation calculateShape()
        {
            //plot.Annotations.Clear();
            return this.fsmodel.returnShapeAnnotation();
                
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

        public void jumpToTimestep(int timestep)
        {
            fsmodel.jumpToTimestep(timestep);
            pause();
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


        public int getCurrentPropertyIndex()
        {
            return fsmodel.getCurrentPropertyIndex();
        }
    }
}
