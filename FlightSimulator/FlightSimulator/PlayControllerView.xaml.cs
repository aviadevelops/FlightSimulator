using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlightSimulator
{

    /// <summary>
    /// Interaction logic for PlaybackScreen.xaml
    /// </summary>
    public partial class PlaybackScreen : Window
    {
        private PlayControllerViewModel playVM;

        public PlaybackScreen(string playBackPath)
        {
            InitializeComponent();
            VMContainer v = new VMContainer();
            DataContext = v;
            playVM = v.VmPlayController;
            JoystickVM = v.VmJoystick;
            playVM.sendPlayBackPathToModel(playBackPath);
        }

        private void btn_open_click(object sender, RoutedEventArgs e)
        {
            if(playVM.load_csv() == null)
            {
                return;
            }
            this.slider_timesteps.Minimum = 0;
            this.slider_timesteps.Maximum = playVM.get_csv_length();

            JoystickVM.initModelIndex();
            this.slider_rudder.Minimum = JoystickVM.rudderMin();
            this.slider_rudder.Maximum = JoystickVM.rudderMax();
            if (this.slider_rudder.Maximum == this.slider_rudder.Minimum)
            {
                this.slider_rudder.Maximum = this.slider_rudder.Minimum + 1;
            }

            this.slider_throttle.Minimum = JoystickVM.ThrottleMin();
            this.slider_throttle.Maximum = JoystickVM.ThrottleMax();
            if (this.slider_throttle.Maximum == this.slider_throttle.Minimum)
            {
                this.slider_throttle.Maximum = this.slider_throttle.Minimum + 1;
            }
        }

       
        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            int length = (int)this.slider_timesteps.Maximum;
            playVM.VM_CurrentTimeStep -= length / 50;
            if (playVM.VM_CurrentTimeStep <= 0)
                playVM.VM_CurrentTimeStep = 0;
        }

        private void btn_pause_click(object sender, RoutedEventArgs e)
        {
            playVM.pause();
        }

        private void btn_foward_click(object sender, RoutedEventArgs e)
        {
            int length = (int)this.slider_timesteps.Maximum;
            playVM.VM_CurrentTimeStep += length / 50;
            if (playVM.VM_CurrentTimeStep >= length)
                playVM.VM_CurrentTimeStep = length;

        }

        private void btn_play_click(object sender, RoutedEventArgs e)
        {
            playVM.play();
        }

        private void txtbox_change_speed_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            playVM.VM_CurrentTimeStep = (int)this.slider_timesteps.Value;
            playVM.VM_CurrentTimeSpan = TimeSpan.FromSeconds((int)this.slider_timesteps.Value/10);
        }

        private void window_flightgear_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            playVM.close();
        }

        private void txtbox_change_speed_pressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                playVM.changeSpeed(this.txtbox_change_speed.Text);
            }
      
        }
    }
}
