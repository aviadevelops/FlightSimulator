using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlightSimulator
{
    public class JoystickControllerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private FlightSimulatorModel fsmodel;




        public JoystickControllerViewModel(FlightSimulatorModel fsmodel)
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


        public List<string> VM_ListBox
        {
            get { return this.fsmodel.ListBox; }
            set {; }
        }

        public IList<DataPoint> VM_Points
        {
            get { return this.fsmodel.Points; }
            set { this.fsmodel.Points = value; }
        }

        public float VM_Rudder
        {
            get { return this.fsmodel.Rudder; }
            set { this.fsmodel.Rudder = value; }
        }

        public string VM_GraphName
        {
            get { return this.fsmodel.GraphName; }
            set { this.fsmodel.GraphName = value; }
        }



        public void setBigCanvasWidthAndHeight(float actualWidth, float actualHeight)
        {
            this.fsmodel.BigEllipseCanvasWidth = actualWidth;
            this.fsmodel.BigEllipseCanvasHeight = actualHeight;
        }

        public void setLittleCanvasWidthAndHeight(float actualWidth, float actualHeight)
        {
            this.fsmodel.LittleEllipseCanvasWidth = actualWidth;
            this.fsmodel.LittleEllipseCanvasHeight = actualHeight;
        }

        public float VM_Throttle
        {
            get { return this.fsmodel.Throttle; }
            set { this.fsmodel.Throttle = value; }
        }

        public float VM_Elevator
        {
            get { return this.fsmodel.Elevator; }
            set
            {
                this.fsmodel.Elevator = value;
            }
        }

        public float VM_Aileron
        {
            get { return this.fsmodel.Aileron; }
            set
            {


                this.fsmodel.Aileron = value;

            }
        }


        public float rudderMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(), this.fsmodel.getRudderIndex());
        }

        public float rudderMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(), this.fsmodel.getRudderIndex());
        }

        public float ThrottleMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(), this.fsmodel.getThrottleIndex());
        }

        public float ThrottleMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(), this.fsmodel.getThrottleIndex());
        }

        public float AileronMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(), this.fsmodel.getAileronIndex());
        }

        public float AileronMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(), this.fsmodel.getAileronIndex());
        }

        public float ElevatorMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(), this.fsmodel.getElevatorIndex());
        }

        public float ElevatorMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(), this.fsmodel.getElevatorIndex());
        }


        public void initModelIndex()
        {
            this.fsmodel.initIndex();

        }

    }
}
