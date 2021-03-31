using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_openApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process objProcess = new Process();
                //Ask user for path
                string path = this.pathTextBox.Text;
                objProcess.StartInfo.FileName = path;
                objProcess.StartInfo.WorkingDirectory = path.Substring(0, path.LastIndexOf('\\'));

                //string playBackPath = path + "\\data\\Protocol";
                //File.Copy(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "playback_small.xml"), System.IO.Path.Combine(playBackPath, "playback_small.xml"));
                //File.Delete("playback_small.xml");

                //Run app with arguments
                objProcess.StartInfo.ArgumentList.Add("--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small");
                objProcess.StartInfo.ArgumentList.Add("--fdm=null");
                objProcess.Start();

                PlaybackScreen w = new PlaybackScreen();

                w.Show();
                this.Close();
            }
            catch (Exception err) //Probably wrong path
            {
                pathTextBox.Text = "ERROR: Could not open app from given path, please try again.";
            }
        }


        /*private void btn_loadPlayback_Click(object sender, RoutedEventArgs e)
        {
            //string playbackPath = this.pathTextBox.Text;

            //File.Copy(System.IO.Path.Combine(playbackPath, "playback_small.xml"), System.IO.Path.Combine(Directory.GetCurrentDirectory(), "playback_small.xml"));

        }*/

        private void btn_fileDialogue_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable files (*.exe)|*.exe";
            if (openFileDialog.ShowDialog() == true)
                pathTextBox.Text = openFileDialog.FileName;
        }

    }
}
