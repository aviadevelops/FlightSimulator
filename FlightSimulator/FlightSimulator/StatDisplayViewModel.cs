using System;
using System.ComponentModel;

namespace FlightSimulator
{
    public class StatDisplayViewModel : INotifyPropertyChanged
    {
        private FlightSimulatorModel fsmodel;
        public event PropertyChangedEventHandler PropertyChanged;

        public StatDisplayViewModel(FlightSimulatorModel fsmodel)
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

        public float VM_Altimeter
        {
            get { return fsmodel.Altimeter; }
            set { fsmodel.Altimeter = value; }
        }

        public float VM_Airspeed
        {
            get { return fsmodel.Airspeed; }
            set { fsmodel.Airspeed = value; }
        }

        public float VM_Heading_deg
        {
            get { return fsmodel.Heading_deg; }
            set { fsmodel.Heading_deg = value; }
        }

        public float VM_Pitch
        {
            get { return fsmodel.Pitch; }
            set { fsmodel.Pitch = value; }
        }

        public float VM_Roll
        {
            get { return fsmodel.Roll; }
            set { fsmodel.Roll = value; }
        }

        public float VM_Yaw
        {
            get { return fsmodel.Yaw; }
            set { fsmodel.Yaw = value; }
        }
    }
}
