using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


/*
 * In PlaybackScreen class we can find buttons for:
 * load a csv train  + load csv test file, play button, pause button, back button, forward button. we can also the the 
 * speed of the flight with the text box change speed. We can jump quickly for every selected moment during the flight,
 * by moving the slider back and forward. We give the slider his max and min value. 
 * As a view this class must contain the appropriate view model.
 */


namespace FlightSimulator
{

    /// <summary>
    /// Interaction logic for PlaybackScreen.xaml
    /// </summary>
    public partial class PlaybackScreen : Window
    {
        private PlayControllerViewModel playVM;
        //private bool isClicked = false;

        public PlaybackScreen(string playBackPath)
        {
            InitializeComponent();
            //this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            
            VMContainer v = new VMContainer();
            DataContext = v;
            playVM = v.VmPlayController;
            JoystickVM = v.VmJoystick;
            StatDisplayVM = v.VmStatDisplay;
            playVM.sendPlayBackPathToModel(playBackPath);
        }

        private void setValues()
        {
            this.slider_timesteps.Minimum = 0;
            this.slider_timesteps.Maximum = 5;
            this.slider_rudder.Minimum = -1;
            this.slider_rudder.Maximum = 1;

            this.slider_throttle.Minimum = 0;
            this.slider_throttle.Maximum = 1;

            this.StatDisplayVM.SetMaxMinAirSpeed();

            this.slider_timesteps.Maximum = playVM.get_csv_length();

            this.JoystickVM.setBigCanvasWidthAndHeight((float)(float)this.JoystickBigCanvas.ActualWidth, (float)this.JoystickBigCanvas.ActualHeight);
            this.JoystickVM.setLittleCanvasWidthAndHeight((float)(float)this.JoystickLittleEllipse.ActualWidth, (float)this.JoystickLittleEllipse.ActualHeight);
        }

        private void btn_open_train_click(object sender, RoutedEventArgs e)
        {
            playVM.load_csv(true);
            anomaliesGraph.Annotations.Clear();
            playVM.VM_CurrentTimeStep = 0;
            playVM.pause();

        }

        private void btn_open_test_click(object sender, RoutedEventArgs e)
        {
            playVM.load_csv(false);
            anomaliesGraph.Annotations.Clear();
            playVM.VM_CurrentTimeStep = 0;
            setValues();
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
                playVM.VM_CurrentTimeStep = length - 1;
        }

        private void btn_play_click(object sender, RoutedEventArgs e)
        {
            try
            {
                OxyPlot.Wpf.Annotation shape = playVM.calculateShape();
                if (shape == null)
                {
                    playVM.play();
                    return;
                }
                anomaliesGraph.Annotations.Clear();
                anomaliesGraph.Annotations.Add(shape);
                anomaliesGraph.InvalidatePlot(true);
            }
            catch (Exception) { }
            playVM.play();
        }

        private void txtbox_change_speed_TextChanged(object sender, TextChangedEventArgs e) {}

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            playVM.VM_CurrentTimeStep = (int)this.slider_timesteps.Value;
            playVM.VM_CurrentTimeSpan = TimeSpan.FromSeconds((int)this.slider_timesteps.Value / 10);
        }

        private void window_flightgear_closing(object sender, CancelEventArgs e)
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

        private void parameterList_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            playVM.displayGraph(parameterList.SelectedItem.ToString());
            anomaliesGraph.Annotations.Clear();
            OxyPlot.Wpf.Annotation shape = playVM.calculateShape();
            this.anomaliesGraph.ResetAllAxes();
            //this.playVM.updateAxes();
            if (shape == null)
                return;
            anomaliesGraph.Annotations.Add(shape);
            anomaliesGraph.InvalidatePlot(true);
        }

        private void anomalies_lb_change(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan timeSpan;
            try
            {
                timeSpan = TimeSpan.Parse(anomalies_lb.SelectedItem.ToString());
            }
            catch (Exception) { return; }
            playVM.jumpToTimestep((int)timeSpan.TotalMilliseconds / 100);
            MessageBox.Show("The flight is paused on time step " + timeSpan + " to resume click play");
        }

        private void btn_load_dll_Click(object sender, RoutedEventArgs e)
        {
            playVM.load_dll();
            int index = playVM.getCurrentPropertyIndex();
            playVM.displayGraph(parameterList.Items[playVM.getCurrentPropertyIndex()].ToString());
            anomaliesGraph.Annotations.Clear();
            OxyPlot.Wpf.Annotation shape = playVM.calculateShape();
            this.anomaliesGraph.ResetAllAxes();
            //this.playVM.updateAxes();
            if (shape == null)
                return;
            anomaliesGraph.Annotations.Add(shape);
            anomaliesGraph.InvalidatePlot(true);
        }
    }
}
