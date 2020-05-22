using OpenHardwareMonitor.Hardware;
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
    /// Interaction logic for Disk.xaml
    /// </summary>
    public partial class Disk : UserControl
    {
        float read = 0;
        float write = 0;
        PerformanceCounter diskread = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
        PerformanceCounter diskwrite = new PerformanceCounter("PhysicalDisk", "Disk Writes/sec", "_Total");
        Computer computer = new Computer() { HDDEnabled = true };
        public Disk()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            computer.Open();
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            DispatcherTimer dp = new DispatcherTimer();
            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
            dp.Tick += (sender2, args) =>
            {
                int number = 0;
                DispatcherTimer dp2 = new DispatcherTimer();
                dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                dp2.Tick += (sender3, args2) =>
                {
                    dp.Stop();
                    NameP.Content = Seting.AnimatedString("DISK", number);
                    number += new Random().Next(0, 4)/3;
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
            };
            dp.Start();
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 0, 100);
            refresh.Tick += (sender2, args) =>
            {
                read += diskread.NextValue();
                write += diskwrite.NextValue();
            };
            refresh.Start();
            DispatcherTimer refresh2 = new DispatcherTimer();
            refresh2.Interval = new TimeSpan(0, 0, 0, 1, 0);
            refresh2.Tick += (sender2, args) =>
            {
                DiskReadValue.Content = Seting.NumberChangerMaximum(read);
                DiskWriteValue.Content = Seting.NumberChangerMaximum(write);
                read = 0;
                write = 0;
            };
            refresh2.Start();
            DispatcherTimer refresh3 = new DispatcherTimer();
            refresh3.Interval = new TimeSpan(0, 0, 0, 5);
            refresh3.Tick += (sender2, args) =>
            {
                TemperatureRefresh();
            };
            refresh3.Start();
            TemperatureRefresh();
        }
        private void TemperatureRefresh()
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                hardware.Update();
                foreach (ISensor sensor in hardware.Sensors)
                {
                    // Celsius is default unit
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        Temperature.Content = sensor.Value.ToString() + "°C";
                        if (sensor.Value > 50)
                        {
                            Temperature.Foreground = Brushes.Red;
                        }
                        else
                        {
                            if (Seting.ColorMode == ColorMode.Dark)
                            {
                                Temperature.Foreground = Brushes.White;
                            }
                            else
                            {
                                Temperature.Foreground = Brushes.Black;
                            }
                        }
                    }

                }
            }
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
                DiskReadName.Foreground = Brushes.White;
                DiskReadValue.Foreground = Brushes.White;
                DiskWriteName.Foreground = Brushes.White;
                DiskWriteValue.Foreground = Brushes.White;
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                DiskReadName.Foreground = Brushes.Black;
                DiskReadValue.Foreground = Brushes.Black;
                DiskWriteName.Foreground = Brushes.Black;
                DiskWriteValue.Foreground = Brushes.Black;
            }

        }
    }
}
