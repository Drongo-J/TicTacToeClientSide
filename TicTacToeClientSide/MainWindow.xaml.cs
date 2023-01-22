using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
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

namespace TicTacToeClientSide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 27001;

        public char Symbol { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            ConnectToServer();
            RequestLoop();
        }

        private void RequestLoop()
        {
            var receiver = Task.Run(() =>
            {
                while (true)
                {
                    ReceiveResponse();
                }
            });
        }

        private void ReceiveResponse()
        {
            var buffer = new Byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);

            if (received == 0)
                return;

            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.UTF8.GetString(data);
            IntegrateView(text);
        }

        private void IntegrateView(string text)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var data = text.Split('\n');
                var row1 = data[0].Split('\t');
                var row2 = data[1].Split('\t');
                var row3 = data[2].Split('\t');

                b1.Content = row1[0];
                b2.Content = row1[1];
                b3.Content = row1[2];

                b4.Content = row2[0];
                b5.Content = row2[1];
                b6.Content = row2[2];

                b7.Content = row3[0];
                b8.Content = row3[1];
                b9.Content = row3[2];
            });
        }

        private void ConnectToServer()
        {
            int attempts = 0;
            while (!ClientSocket.Connected)
            {
                try
                {
                    ++attempts;
                    ClientSocket.Connect(IPAddress.Parse("192.168.1.67"), PORT);
                }
                catch (Exception)
                {

                }
            }
            MessageBox.Show("Connected");

            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0)
                return;

            var data = new byte[received];
            Array.Copy(buffer, data, received);

            string text = Encoding.UTF8.GetString(data);
            //Symbol = text[0];
            this.Title = "Player : " + text;
            player.Text = this.Title;
        }

        private void SendString(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private void ClickCommand(object content)
        {
            Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    string request = content.ToString() + player.Text.Split(':').ElementAt(1).Trim();
                    SendString(request);
                });
            });
        }

        // for test 
        private void b1_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b5_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b6_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b7_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b8_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }

        private void b9_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            ClickCommand(bt.Content);
        }
    }
}
