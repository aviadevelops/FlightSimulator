using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    public class VMContainer
    {
        private PlayControllerViewModel vm1;
        private JoystickControllerViewModel vm2;
        private FlightSimulatorModel fsmodel;

        public VMContainer()
        {
            this.fsmodel = new FlightSimulatorModel();
            this.fsmodel.connect();
            this.vm1 = new PlayControllerViewModel(this.fsmodel);
            this.vm2 = new JoystickControllerViewModel(this.fsmodel);

        }

        public PlayControllerViewModel VmPlayController {
            get { return this.vm1; } 
        }
        public JoystickControllerViewModel VmJoystick
        {
            get { return this.vm2; }
        }

    }
}
