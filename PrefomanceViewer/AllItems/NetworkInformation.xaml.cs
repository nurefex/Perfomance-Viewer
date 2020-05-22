using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace PrefomanceViewer.AllItems
{
    /// <summary>
    /// Interaction logic for NetworkInformation.xaml
    /// </summary>
    public partial class NetworkInformation : UserControl
    {
        public NetworkInformation()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorChange();
            Refresh();
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
                        NameP.Content = Seting.AnimatedString("NETWORK INFORMATION", number);
                        number++;
                        if (number >= NameP.Content.ToString().Length + 1)
                        {
                            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
                            dp2.Stop();
                            dp.Start();
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
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 1, 0);
            refresh.Tick += (sender2, args) =>
              {
                  Refresh();
              };
            refresh.Start();
            Seting.ColorModeChange += (sender2, args) =>
                {
                    ColorChange();
                };
        }
        private void Refresh()
        {
            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                IpAddress.Visibility = Visibility.Visible;
                IpAddressLabel.Visibility = Visibility.Visible;
                DNSIP.Visibility = Visibility.Visible;
                DNSIPLabel.Visibility = Visibility.Visible;
                DG.Visibility = Visibility.Visible;
                DGLabel.Visibility = Visibility.Visible;
                GlobalIp.Visibility = Visibility.Visible;
                GlobalIpLabel.Visibility = Visibility.Visible;
                error.Visibility = Visibility.Collapsed;
                NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface Interface in Interfaces)
                {
                    if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    IPInterfaceProperties ipProperties = Interface.GetIPProperties();
                    IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;
                    foreach (IPAddress dnsAdress in dnsAddresses)
                    {
                        DNSIP.Content = dnsAdress;
                    }
                    UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                    {
                        IpAddress.Content = UnicatIPInfo.Address + "/";
                    }
                }
                /*new Thread(() =>
                {
                    try
                    {
                        string externalip = new WebClient().DownloadString("http://icanhazip.com");
                        GlobalIp.Content = externalip;
                    }
                    catch(Exception)
                    {

                    }
                }).Start();*/
                DG.Content = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.OperationalStatus == OperationalStatus.Up).Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback).SelectMany(n => n.GetIPProperties()?.GatewayAddresses).Select(g => g?.Address).Where(a => a != null).FirstOrDefault();
                if (IpAddress.Content == "/")
                {
                    IpAddress.Content = "No have Ip address";
                }
                if (DNSIP.Content == "")
                {
                    DNSIP.Content = "No have DNS address";
                }
                if (DG.Content == "")
                {
                    DG.Content = "No have Default Getway Address";
                }
            }
            else
            {
                IpAddress.Visibility = Visibility.Collapsed;
                IpAddressLabel.Visibility = Visibility.Collapsed;
                DNSIP.Visibility = Visibility.Collapsed;
                DNSIPLabel.Visibility = Visibility.Collapsed;
                DG.Visibility = Visibility.Collapsed;
                DGLabel.Visibility = Visibility.Collapsed;
                GlobalIp.Visibility = Visibility.Collapsed;
                GlobalIpLabel.Visibility = Visibility.Collapsed;
                error.Visibility = Visibility.Visible;
            }
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
                IpAddress.Foreground = Brushes.White;
                IpAddressLabel.Foreground = Brushes.White;
                DNSIP.Foreground = Brushes.White;
                DNSIPLabel.Foreground = Brushes.White;
                DG.Foreground = Brushes.White;
                DGLabel.Foreground = Brushes.White;
                GlobalIp.Foreground = Brushes.White;
                GlobalIpLabel.Foreground = Brushes.White;
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                IpAddress.Foreground = Brushes.Black;
                IpAddressLabel.Foreground = Brushes.Black;
                DNSIP.Foreground = Brushes.Black;
                DNSIPLabel.Foreground = Brushes.Black;
                DG.Foreground = Brushes.Black;
                DGLabel.Foreground = Brushes.Black;
                GlobalIp.Foreground = Brushes.Black;
                GlobalIpLabel.Foreground = Brushes.Black;
            }
        }
    }
}