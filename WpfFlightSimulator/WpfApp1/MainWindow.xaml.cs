using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
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
            Process objProcess = new Process();
            //Ask user for path
            string path = this.pathTextBox.Text;
            objProcess.StartInfo.FileName = path;
            objProcess.StartInfo.WorkingDirectory = path.Substring(0, path.Length - 9);

            //string playBackPath = path + "\\data\\Protocol";
            //File.Copy(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "playback_small.xml"), System.IO.Path.Combine(playBackPath, "playback_small.xml"));
            //File.Delete("playback_small.xml");


            objProcess.StartInfo.ArgumentList.Add("--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small");
            objProcess.StartInfo.ArgumentList.Add("--fdm=null");
            objProcess.Start();
        }

        private void btn_loadCSV_Click(object sender, RoutedEventArgs e)
        {
            string path = this.pathTextBox.Text;
            var lines = File.ReadLines(path);


        connection:


            try
            {
                TcpClient client = new TcpClient("localhost", 5400);
                int byteCount;
                string msg;
                Console.WriteLine("sending data to server...");
                NetworkStream stream = client.GetStream();

                foreach (string line in lines)
                {
                    msg = line + "\n";
                    byteCount = Encoding.ASCII.GetByteCount(msg + 1);
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    stream.Write(sendData, 0, sendData.Length);
                    Console.WriteLine(msg);
                    Thread.Sleep(100);
                }



                StreamReader sr = new StreamReader(stream);
                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed");
                goto connection;
            }
        }

        private void btn_loadPlayback_Click(object sender, RoutedEventArgs e)
        {
            //string playbackPath = this.pathTextBox.Text;

            //File.Copy(System.IO.Path.Combine(playbackPath, "playback_small.xml"), System.IO.Path.Combine(Directory.GetCurrentDirectory(), "playback_small.xml"));

        }

        private void btn_moveToNextWindow_Click(object sender, RoutedEventArgs e)
        {
            window2 w = new window2();

            w.Show();
            this.Close();
        }

        private void btn_fileDialogue_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                pathTextBox.Text = openFileDialog.FileName;
        }

    }
}
