using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public partial class Drive : UserControl
    {
        float read = 0;
        float write = 0;
        PerformanceCounter diskread = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
        PerformanceCounter diskwrite = new PerformanceCounter("PhysicalDisk", "Disk Writes/sec", "_Total");
        Computer computer = new Computer() { HDDEnabled = true };
        public Drive()
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
            computer.Open();
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            DispatcherTimer dp3 = new DispatcherTimer();
            dp3.Interval = new TimeSpan(0, 5, 0);
            dp3.Tick += (sender2, args2) =>
            {
                int index2 = 0;
                int index = 0;
                DriveInfo[] alldrive = DriveInfo.GetDrives();
                DispatcherTimer dp4 = new DispatcherTimer();
                dp4.Interval = new TimeSpan(0, 0, 0, 3);
                dp4.Tick += (sender3, args3) =>
                {
                    if (index < alldrive.Length)
                    {
                        try
                        {
                            if (index == 0)
                            {
                                informationpanel.Visibility = Visibility.Visible;
                                valuepanel.Visibility = Visibility.Hidden;
                            }
                            Grid grid = new Grid();
                            grid.VerticalAlignment = VerticalAlignment.Top;
                            grid.Margin = new Thickness(0, index * 20, 0, 0);
                            Label Name = new Label();
                            Name.Content = alldrive[index2].Name.ToUpper()[0];
                            Name.FontSize = 9;
                            Name.VerticalAlignment = VerticalAlignment.Top;
                            Name.HorizontalAlignment = HorizontalAlignment.Left;
                            Name.Margin = new Thickness(-2, 1, 0, 0);
                            if (Seting.ColorMode == ColorMode.Dark)
                            {
                                Name.Foreground = Brushes.White;
                            }
                            else
                            {
                                Name.Foreground = Brushes.Black;
                            }
                            grid.Children.Add(Name);
                            ProgressBar progressBar = new ProgressBar();
                            progressBar.VerticalAlignment = VerticalAlignment.Top;
                            progressBar.HorizontalAlignment = HorizontalAlignment.Left;
                            progressBar.Width = 55;
                            progressBar.Height = 18;
                            progressBar.Background = Brushes.Transparent;
                            progressBar.Margin = new Thickness(14, 2, 0, 0);
                            progressBar.Value = 100 - (Convert.ToDouble(alldrive[index].AvailableFreeSpace) / Convert.ToDouble(alldrive[index].TotalSize) * 100.0);
                            if (progressBar.Value >= 90)
                            {
                                progressBar.Foreground = Brushes.Red;
                            }
                            else
                            {
                                progressBar.Foreground = Brushes.Blue;
                            }
                            grid.Children.Add(progressBar);
                            Label values = new Label();
                            values.VerticalAlignment = VerticalAlignment.Top;
                            values.HorizontalAlignment = HorizontalAlignment.Center;
                            values.FontSize = 7;
                            values.Margin = new Thickness(11, 1, 0, 0);
                            if (Seting.ColorMode == ColorMode.Dark)
                            {
                                values.Foreground = Brushes.White;
                            }
                            else
                            {
                                values.Foreground = Brushes.Black;
                            }
                            values.Content = Seting.NumberChangerMaximum(alldrive[index].TotalSize - alldrive[index].AvailableFreeSpace) + "/" + Seting.NumberChangerMaximum(alldrive[index].TotalSize);
                            grid.Children.Add(values);
                            informationpanel.Children.Add(grid);
                            automaticmargin.ScrollToEnd();
                            index2++;
                        }
                        catch (Exception)
                        {

                        }
                        index++;
                    }
                    else
                    {
                        informationpanel.Visibility = Visibility.Hidden;
                        valuepanel.Visibility = Visibility.Visible;
                        informationpanel.Children.Clear();
                        dp4.Stop();
                    }
                };
                dp4.Start();
            };
            dp3.Start();
            DispatcherTimer dp = new DispatcherTimer();
            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
            dp.Tick += (sender2, args) =>
            {
                if (Seting.AnimatedStringSetting)
                {
                    int number = 0;
                    DispatcherTimer dp2 = new DispatcherTimer();
                    dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                    dp2.Tick += (sender3, args2) =>
                    {
                        dp.Stop();
                        NameP.Content = Seting.AnimatedString("DRIVE", number);
                        number += new Random().Next(0, 4) / 3;
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
                DiskReadValue.Foreground = Brushes.White;
                DiskWriteValue.Foreground = Brushes.White;
                if (Temperature.Foreground != Brushes.Red)
                {
                    Temperature.Foreground = Brushes.White;
                }
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                DiskReadValue.Foreground = Brushes.Black;
                DiskWriteValue.Foreground = Brushes.Black;
                if (Temperature.Foreground != Brushes.Red)
                {
                    Temperature.Foreground = Brushes.Black;
                }
            }

        }
    }
}