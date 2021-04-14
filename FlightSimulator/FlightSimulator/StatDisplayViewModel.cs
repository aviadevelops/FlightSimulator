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


        public int VM_MaxSpeed
        {
            get { return this.fsmodel.MaxSpeed; }
            set
            {
                this.fsmodel.MaxSpeed = value;
            }
        }

        public int VM_MinSpeed
        {
            get { return this.fsmodel.MinSpeed; }
            set
            {
                this.fsmodel.MinSpeed = value;
            }
        }

        public int VM_MaxYaw
        {
            get { return fsmodel.MaxYaw; }
            set
            {
                fsmodel.MaxYaw = value;

            }
        }

        public int VM_MinYaw
        {
            get { return fsmodel.MinYaw; }
            set
            {
                fsmodel.MinYaw = value;
            }
        }

        public int VM_MaxRoll
        {
            get { return fsmodel.MaxRoll; }
            set
            {
                fsmodel.MaxRoll = value;

            }
        }

        public int VM_MinRoll
        {
            get { return fsmodel.MinRoll; }
            set
            {
                fsmodel.MinRoll = value;

            }
        }

        public int VM_MaxPitch
        {
            get { return fsmodel.MaxPitch; }
            set
            {
                fsmodel.MaxPitch = value;
            }
        }

        public int VM_MinPitch
        {
            get { return fsmodel.MinPitch; }
            set
            {
                fsmodel.MinPitch = value;

            }
        }


        public int VM_MaxAlt
        {
            get { return fsmodel.MaxAlt; }
            set
            {
                fsmodel.MaxAlt = value;
            }
        }

        public int VM_MinAlt
        {
            get { return fsmodel.MinAlt; }
            set
            {
                fsmodel.MinAlt = value;
            }
        }


        public int VM_MaxDeg
        {
            get { return fsmodel.MaxDeg; }
            set
            {
                fsmodel.MaxDeg = value;
            }
        }

        public int VM_MinDeg
        {
            get { return fsmodel.MinDeg; }
            set
            {
                fsmodel.MinDeg = value;
            }
        }

        public void SetMaxMinAirSpeed()
        {

            this.fsmodel.SetGaugesMinMax();

        }
    }
}
