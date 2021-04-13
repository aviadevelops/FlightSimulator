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

        public IList<DataPoint> VM_PointsLeft
        {
            get { return this.fsmodel.PointsLeft; }
            set { this.fsmodel.PointsLeft = value; }
        }

        public IList<DataPoint> VM_PointsRight
        {
            get { return this.fsmodel.PointsRight; }
            set { this.fsmodel.PointsRight = value; }
        }

        //public IList<DataPoint> VM_PointsBottomLine
        //{
        //    get { return this.fsmodel.PointsBottomLine; }
        //    set { this.fsmodel.PointsBottomLine = value; }
        //}

        public IList<DataPoint> VM_PointsBottomGray
        {
            get { return this.fsmodel.PointsBottomGray; }
            set { this.fsmodel.PointsBottomGray = value; }
        }

        public IList<DataPoint> VM_PointsBottomRed
        {
            get { return this.fsmodel.PointsBottomRed; }
            set { this.fsmodel.PointsBottomRed = value; }
        }

        public float VM_Rudder
        {
            get { return this.fsmodel.Rudder; }
            set { this.fsmodel.Rudder = value; }
        }

        public string VM_GraphNameLeft
        {
            get { return this.fsmodel.GraphNameLeft; }
            set { this.fsmodel.GraphNameLeft = value; }
        }

        public string VM_GraphNameRight
        {
            get { return this.fsmodel.GraphNameRight; }
            set { this.fsmodel.GraphNameRight = value; }
        }

        public string VM_GraphNameBottom
        {
            get { return this.fsmodel.GraphNameBottom; }
            set { this.fsmodel.GraphNameBottom = value; }
        }

        public List<TimeSpan> VM_ErrorsInGraph
        {
            get { return this.fsmodel.ErrorsInGraph; }
            set { this.fsmodel.ErrorsInGraph = value; }
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
            return this.fsmodel.findMax(this.fsmodel.getLines(false), this.fsmodel.getRudderIndex());
        }

        public float rudderMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(false), this.fsmodel.getRudderIndex());
        }

        public float ThrottleMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(false), this.fsmodel.getThrottleIndex());
        }

        public float ThrottleMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(false), this.fsmodel.getThrottleIndex());
        }

        public float AileronMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(false), this.fsmodel.getAileronIndex());
        }

        public float AileronMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(false), this.fsmodel.getAileronIndex());
        }

        public float ElevatorMax()
        {
            return this.fsmodel.findMax(this.fsmodel.getLines(false), this.fsmodel.getElevatorIndex());
        }

        public float ElevatorMin()
        {
            return this.fsmodel.findMin(this.fsmodel.getLines(false), this.fsmodel.getElevatorIndex());
        }


        public void initModelIndex()
        {
            this.fsmodel.initIndex();

        }

    }
}
