﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FlightSimulator
{

    /// <summary>
    /// Interaction logic for PlaybackScreen.xaml
    /// </summary>
    public partial class PlaybackScreen : Window
    {
        private PlayControllerViewModel playVM;
        private bool dragStarted = false;

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

        private void btn_open_click(object sender, RoutedEventArgs e)
        {
            if (playVM.load_csv() == null)
            {
                return;
            }
            this.slider_timesteps.Minimum = 0;
            this.slider_timesteps.Maximum = playVM.get_csv_length();

            JoystickVM.initModelIndex();
            this.slider_rudder.Minimum = -1;
            this.slider_rudder.Maximum = 1;

            this.slider_throttle.Minimum = 0;
            this.slider_throttle.Maximum = 1;

            this.JoystickVM.setBigCanvasWidthAndHeight((float)(float)this.JoystickBigCanvas.ActualWidth, (float)this.JoystickBigCanvas.ActualHeight);
            this.JoystickVM.setLittleCanvasWidthAndHeight((float)(float)this.JoystickLittleEllipse.ActualWidth, (float)this.JoystickLittleEllipse.ActualHeight);

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
            int x = 0;
            playVM.displayGraph(parameterList.SelectedItems[x].ToString());
        }
    }
}
