using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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

            this.slider_timesteps.Maximum = playVM.get_csv_length();

            this.JoystickVM.setBigCanvasWidthAndHeight((float)(float)this.JoystickBigCanvas.ActualWidth, (float)this.JoystickBigCanvas.ActualHeight);
            this.JoystickVM.setLittleCanvasWidthAndHeight((float)(float)this.JoystickLittleEllipse.ActualWidth, (float)this.JoystickLittleEllipse.ActualHeight);
        }

        private void btn_open_train_click(object sender, RoutedEventArgs e)
        {
            playVM.load_csv(true);
            playVM.VM_CurrentTimeStep = 0;
        }

        private void btn_open_test_click(object sender, RoutedEventArgs e)
        {
            playVM.load_csv(false);
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
                playVM.VM_CurrentTimeStep = length;
        }

        private void btn_play_click(object sender, RoutedEventArgs e)
        {
            playVM.initialize();
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            playVM.displayGraph(parameterList.SelectedItems[0].ToString());
            anomaliesGraph.Annotations.Clear();
            OxyPlot.Wpf.FunctionAnnotation shape = (OxyPlot.Wpf.FunctionAnnotation)playVM.calculateShape();
            if (shape == null)
                return;
            anomaliesGraph.Annotations.Add(shape);
            anomaliesGraph.InvalidatePlot(true);
        }

        private void btn_load_dll_Click(object sender, RoutedEventArgs e)
        {

            playVM.load_dll();
        }

        //private void btn_load_dll_click(object sender, RoutedEventArgs e)
        //{
        //    if (!isClicked)
        //    {
        //        AnomaliesWindow window = new AnomaliesWindow(playVM);

        //        window.Show();
        //    }
        //    isClicked = true;
        //}
    }
}
