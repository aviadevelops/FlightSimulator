using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public float VM_Rudder
        {
            get { return this.fsmodel.Rudder; }
            set { this.fsmodel.Rudder = value; }
        }

        public float VM_Throttle
        {
            get { return this.fsmodel.Throttle; }
            set { this.fsmodel.Throttle = value; }
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

        public void initModelIndex()
        {
            this.fsmodel.initIndex();

        }


    }
}
