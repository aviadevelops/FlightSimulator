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
    public partial class PlaybackScreen : Window, INotifyPropertyChanged
    {
        private TcpClient client;
        private NetworkStream stream;
        private int sleepFor = 100;
        private int currentTimeStep = 0;
        private string[] lines;
        private bool isPaused = true;
        private bool isDone = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Time_Step
        {
            get { return currentTimeStep; }
            set
            {
                currentTimeStep = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "Time_Step")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public PlaybackScreen()
        {
            InitializeComponent();
            DataContext = this;

        connection:
            try
            {
                client = new TcpClient("localhost", 5400);
                Console.WriteLine("sending data to server...");
                stream = client.GetStream();
            }
            catch (Exception)
            {
                Console.WriteLine("Connection failed");
                goto connection;
            }
        }

        private void btn_open_click(object sender, RoutedEventArgs e)
        {
            string path = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            if (path == null)
            {
                string text = "Please choose a path";
                MessageBox.Show(text);
                return;
            }
            isDone = true;
            lines = File.ReadAllLines(path);
            this.slider_timesteps.Minimum = 0;
            this.slider_timesteps.Maximum = File.ReadAllLines(path).Length - 1;
            currentTimeStep = 0;
        }

        private void sendInLoop(string[] lines, int max)
        {
            string msg;
            isDone = false;
            while (true)
            {
                for (; currentTimeStep <= max; ++currentTimeStep)
                {
                    if (isPaused)
                        break;
                    msg = lines[currentTimeStep] + "\n";
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    stream.Write(sendData, 0, sendData.Length);
                    Console.WriteLine(msg);
                    Thread.Sleep(sleepFor);
                }
                if (isDone)
                    break;
            }
        }

        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            int length = (int)this.slider_timesteps.Maximum;
            currentTimeStep -= length / 50;
            if (currentTimeStep <= 0)
                currentTimeStep = 0;
        }

        private void btn_pause_click(object sender, RoutedEventArgs e)
        {
            isPaused = true;
        }

        private void btn_foward_click(object sender, RoutedEventArgs e)
        {
            int length = (int)this.slider_timesteps.Maximum;
            currentTimeStep += length / 50;
            if (currentTimeStep >= length)
                currentTimeStep = length;

        }

        private void btn_play_click(object sender, RoutedEventArgs e)
        {
            isPaused = false;
            int max = (int)this.slider_timesteps.Maximum;
            var thread = new Thread(() => sendInLoop(lines, max));
            thread.Start();
        }

        private void txtbox_change_speed_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentTimeStep = (int)this.slider_timesteps.Value;
        }

        private void window_flightgear_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isDone = true;
            stream.Close();
            client.Close();
        }

        private void txtbox_change_speed_pressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                try
                {
                    double temp = Convert.ToDouble(this.txtbox_change_speed.Text);
                    if (temp <= 0)
                        throw new Exception();
                    temp = 100.0 / temp;
                    sleepFor = (int)temp;
                }
                catch (Exception) {; }
            }
        }
    }
}
