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
    /// Interaction logic for GPU.xaml
    /// </summary>
    public partial class GPU : UserControl
    {
        //PerformanceCounter gpuusing = new PerformanceCounter("Win32_VideoController", "StatusInfo");
        Computer computer = new Computer() { GPUEnabled = true };
        public GPU()
        {
            InitializeComponent();
        }
        /*public void Refresh()
        {
            float precessorusingtest = gpuusing.RawValue;
            if (precessorusingtest >= 90)
            {
                Percent.Foreground = Brushes.Red;
            }
            else
            {
                if (Seting.ColorMode == ColorMode.Dark)
                {
                    Percent.Foreground = Brushes.White;
                }
                else
                {
                    Percent.Foreground = Brushes.Black;
                }
            }
            Percent.Content = "" + Math.Round(precessorusingtest, 0) + "%";
        }*/
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorChange();
            computer.Open();
            DispatcherTimer dp = new DispatcherTimer();
            dp.Interval = new TimeSpan(0, 0, 0, new Random().Next(10, 61));
            dp.Tick += (sender2, args) =>
            {
                dp.Stop();
                int number = 0;
                DispatcherTimer dp2 = new DispatcherTimer();
                dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                dp2.Tick += (sender3, args2) =>
                {
                    NameP.Content = Seting.AnimatedString("GPU", number);
                    number += new Random().Next(0, 4)/3;
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
            };
            dp.Start();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            //Refresh();
            TemperatureRefresh();
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 0, 500);
            refresh.Tick += (sender2, args) =>
            {
                //Refresh();
            };
            refresh.Start();
            DispatcherTimer refresh2 = new DispatcherTimer();
            refresh2.Interval = new TimeSpan(0, 0, 0, 5);
            refresh2.Tick += (sender2, args) =>
            {
                TemperatureRefresh();
            };
            refresh2.Start();
        }

        private void TemperatureRefresh()
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                hardware.Update();
                float temperature = 0;
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature && hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                    {
                        temperature = Convert.ToSingle(sensor.Value);

                    }
                }
                if (temperature != 0)
                {

                    Temperature.Content = temperature + "°C";
                    if (temperature > 80)
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

        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
            }
            else
            {
                NameP.Foreground = Brushes.Black;
            }
        }
    }
}
