
namespace FlightSimulator
{
    public class VMContainer
    {
        private PlayControllerViewModel vm1;
        private JoystickControllerViewModel vm2;
        private StatDisplayViewModel vm3;
        //private GraphDisplayViewModel vm4;
        private FlightSimulatorModel fsmodel;

        public VMContainer()
        {
            this.fsmodel = new FlightSimulatorModel();
            this.fsmodel.connect();
            this.vm1 = new PlayControllerViewModel(this.fsmodel);
            this.vm2 = new JoystickControllerViewModel(this.fsmodel);
            this.vm3 = new StatDisplayViewModel(this.fsmodel);
            //this.vm4 = new GraphDisplayViewModel(this.fsmodel);

        }

        public PlayControllerViewModel VmPlayController 
        {
            get { return this.vm1; } 
        }
        public JoystickControllerViewModel VmJoystick
        {
            get { return this.vm2; }
        }
        public StatDisplayViewModel VmStatDisplay
        {
            get { return this.vm3; }
        }
        //public GraphDisplayViewModel VmGraphDisplay
        //{
        //    get { return this.vm4; }
        //}

    }
}
