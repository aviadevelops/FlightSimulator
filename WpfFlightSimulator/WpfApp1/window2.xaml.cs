using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for window2.xaml
    /// </summary>
    public partial class window2 : Window
    {
        public window2()
        {
            InitializeComponent();
        }

        private void btn_moveToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();

            w.Show();
            this.Close();
        }

        private void btn_fileDialogue_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;

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
            catch (Exception)
            {
                Console.WriteLine("Connection failed");
                goto connection;
            }
        }
    }
}
