using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Threading;

namespace PrefomanceViewer.AllItems
{
    /// <summary>
    /// Interaction logic for Network.xaml
    /// </summary>
    public partial class Network : UserControl
    {
        struct Value
        {
            public string IpAddress;
            public string Mask;
            public string DefaultGateway;
            public string DNS;
            public string RemoteIpAddress;
        }
        float send = 0;
        float recived = 0;
        public Network()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage a1 = new BitmapImage();
            a1.BeginInit();
            a1.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\UpLoadIcon.png");
            a1.EndInit();
            pictrure1.Source = a1;
            BitmapImage a2 = new BitmapImage();
            a2.BeginInit();
            a2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\DownLoadIcon.png");
            a2.EndInit();
            pictrure2.Source = a2;
            DispatcherTimer dp3 = new DispatcherTimer();
            dp3.Interval = new TimeSpan(0, 5, 0);
            dp3.Tick += (sender2, args2) =>
            {
                int number = 0;
                List<Value> AllInformation = new List<Value>();
                int index = 0;
                DispatcherTimer dp4 = new DispatcherTimer();
                dp4.Interval = new TimeSpan(0, 0, 0, 3);
                dp4.Tick += (sender3, args3) =>
                {
                    if (number == 0)
                    {
                        valuepanel.Visibility = Visibility.Hidden;
                        informationpanel.Visibility = Visibility.Visible;
                        AddNewDataScreen("Ip Address: ", AllInformation[index].IpAddress);
                    }
                    else if (number == 1)
                    {
                        AddNewDataScreen("\nMask: ", AllInformation[index].Mask);
                    }
                    else if (number == 2)
                    {
                        AddNewDataScreen("\nDefault Gateway: ", AllInformation[index].DefaultGateway);
                    }
                    else if (number == 3)
                    {
                        AddNewDataScreen("\nDNS: ", AllInformation[index].DNS);
                    }
                    else if (number == 4)
                    {
                        AddNewDataScreen("\nRemote Ip Address: ", AllInformation[index].RemoteIpAddress);
                    }
                    else
                    {
                        if (index < AllInformation.Count() - 1)
                        {
                            number = -1;
                            index++;
                            processorvaluenpanel.Text += "\n---------------\n";
                        }
                        else
                        {
                            dp4.Stop();
                            valuepanel.Visibility = Visibility.Visible;
                            informationpanel.Visibility = Visibility.Hidden;
                            processorvaluenpanel.Text = "";
                        }
                    }
                    number++;
                    automaticmargin.ScrollToEnd();
                };
                dp4.Start();
            };
            //dp3.Start();
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NetworkRecivedValue.Foreground = Brushes.White;
                NetworkSendValue.Foreground = Brushes.White;
            }
            else
            {
                NetworkRecivedValue.Foreground = Brushes.Black;
                NetworkSendValue.Foreground = Brushes.Black;
            }
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            DispatcherTimer dp = new DispatcherTimer();
            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
            dp.Tick += (sender2, args) =>
            {
                if (Seting.AnimatedStringSetting)
                {
                    dp.Stop();
                    int number = 0;
                    DispatcherTimer dp2 = new DispatcherTimer();
                    dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                    dp2.Tick += (sender3, args2) =>
                    {
                        NameP.Content = Seting.AnimatedString("NETWORK", number);
                        number += new Random().Next(0, 2);
                        if (number >= NameP.Content.ToString().Length + 1)
                        {
                            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
                            dp.Start();
                            dp2.Stop();
                        }
                        else
                        {
                            dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                        }
                    };
                    dp2.Start();
                }
            };
            dp.Start();
            PerformanceCounterCategory pcg = new PerformanceCounterCategory("Network Interface");
            string instance = pcg.GetInstanceNames()[0];
            PerformanceCounter networksent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter networkreceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 0, 100);
            refresh.Tick += (sender2, args) =>
            {
                send += networksent.NextValue();
                recived += networkreceived.NextValue();
            };
            refresh.Start();
            DispatcherTimer refresh2 = new DispatcherTimer();
            refresh2.Interval = new TimeSpan(0, 0, 0, 1, 0);
            refresh2.Tick += (sender2, args) =>
            {
                NetworkRecivedValue.Content = Seting.NumberChangerMaximum(recived / 8);
                NetworkSendValue.Content = Seting.NumberChangerMaximum(send / 8);
                recived = 0;
                send = 0;
            };
            refresh2.Start();
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
                NetworkSendValue.Foreground = Brushes.White;
                NetworkRecivedValue.Foreground = Brushes.White;
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                NetworkSendValue.Foreground = Brushes.Black;
                NetworkRecivedValue.Foreground = Brushes.Black;
            }
        }
        private void AddNewDataScreen(string H1, string H2)
        {
            processorvaluenpanel.Inlines.Add(new Run(H1) { FontWeight = FontWeights.Bold });
            processorvaluenpanel.Inlines.Add(new Run(H2));
        }
    }
}