using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;

namespace PrefomanceViewer.AllItems
{
    /// <summary>
    /// Interaction logic for Processor.xaml
    /// </summary>
    public partial class Processor : UserControl
    {
        public struct Value
        {
            public string Name;
            public int CoreNumber;
            public int ClockSpeed;
            public bool Virtualizition;
        }
        PerformanceCounter processorusing = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        Computer computer = new Computer() { CPUEnabled = true };
        public Processor()
        {
            InitializeComponent();
        }
        public void Refresh()
        {
            float precessorusingtest = processorusing.NextValue();
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
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorChange();
            computer.Open();
            DispatcherTimer dp3 = new DispatcherTimer();
            dp3.Interval = new TimeSpan(0, 5, 0);
            dp3.Tick += (sender2, args2) =>
              {
                  int number = 0;
                  ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_Processor");
                  List<Value> AllInformation = new List<Value>();
                  foreach (ManagementObject obj in myVideoObject.Get())
                  {
                      Value test = new Value();
                      try
                      {
                          test.Name = obj["Name"].ToString();
                          test.CoreNumber = Convert.ToInt32(obj["NumberOfCores"]);
                          test.ClockSpeed = Convert.ToInt32(obj["CurrentClockSpeed"]);
                          test.Virtualizition = Convert.ToBoolean(obj["VirtualizationFirmwareEnabled"]);
                      }
                      catch (Exception)
                      {

                      }
                      AllInformation.Add(test);
                  }
                  int index = 0;
                  DispatcherTimer dp4 = new DispatcherTimer();
                  dp4.Interval = new TimeSpan(0, 0, 0, 3);
                  dp4.Tick += (sender3, args3) =>
                    {
                        if (number == 0)
                        {
                            processorvaluepanel.Visibility = Visibility.Hidden;
                            processorinformationpanel.Visibility = Visibility.Visible;
                            AddNewDataScreen("Name: ", AllInformation[index].Name);
                        }
                        else if (number == 1)
                        {
                            AddNewDataScreen("\nCore: ", AllInformation[index].CoreNumber.ToString());
                        }
                        else if (number == 2)
                        {
                            string clockspeedstr = "";
                            if (AllInformation[index].ClockSpeed >= 1000)
                            {
                                clockspeedstr = Math.Round(AllInformation[index].ClockSpeed / 1000.0, 2) + " GHz";
                            }
                            else
                            {
                                clockspeedstr = AllInformation[index].ClockSpeed + " MHz";
                            }
                            AddNewDataScreen("\nClock Speed: ", clockspeedstr);
                        }
                        else if (number == 3)
                        {
                            AddNewDataScreen("\nVirtualization: ", (AllInformation[index].Virtualizition ? "enable" : "disable"));
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
                                processorvaluepanel.Visibility = Visibility.Visible;
                                processorinformationpanel.Visibility = Visibility.Hidden;
                                processorvaluenpanel.Text = "";
                            }
                        }
                        number++;
                        automaticmargin.ScrollToEnd();
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
                      dp.Stop();
                      int number = 0;
                      DispatcherTimer dp2 = new DispatcherTimer();
                      dp2.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(100, 250));
                      dp2.Tick += (sender3, args2) =>
                        {
                            NameP.Content = Seting.AnimatedString("PROCESSOR", number);
                            number += new Random().Next(0, 2);
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
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            Refresh();
            TemperatureRefresh();
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 0, 500);
            refresh.Tick += (sender2, args) =>
            {
                Refresh();
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
                    if (sensor.SensorType == SensorType.Temperature && hardware.HardwareType == HardwareType.CPU)
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
                processorvaluenpanel.Foreground = Brushes.White;
                if (Percent.Foreground != Brushes.Red)
                {
                    Percent.Foreground = Brushes.White;
                }
                if (Temperature.Foreground != Brushes.Red)
                {
                    Temperature.Foreground = Brushes.White;
                }
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                processorvaluenpanel.Foreground = Brushes.Black;
                if (Percent.Foreground != Brushes.Red)
                {
                    Percent.Foreground = Brushes.Black;
                }
                if (Temperature.Foreground != Brushes.Red)
                {
                    Temperature.Foreground = Brushes.Black;
                }
            }
        }
        private void AddNewDataScreen(string H1, string H2)
        {
            processorvaluenpanel.Inlines.Add(new Run(H1) { FontWeight = FontWeights.Bold });
            processorvaluenpanel.Inlines.Add(new Run(H2));
        }
    }
}