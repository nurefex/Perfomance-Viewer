using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
    /// Interaction logic for Memory.xaml
    /// </summary>
    public partial class Memory : UserControl
    {
        public struct Value
        {
            public string Name;
            public string Type;
            public int Speed;
            public byte CLI;
            public float Size;
        }
        PerformanceCounter memoryusing = new PerformanceCounter("Memory", "Available Bytes");
        public Memory()
        {
            InitializeComponent();
        }
        public void Refresh()
        {
            ulong maxmemory = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
            ulong freememory = Convert.ToUInt64(memoryusing.NextValue());
            if (Convert.ToDouble(maxmemory - freememory) / maxmemory * 100.0 >= 90)
            {
                Used.Foreground = Brushes.Red;
            }
            else
            {
                if (Seting.ColorMode == ColorMode.Dark)
                {
                    Used.Foreground = Brushes.White;
                }
                else
                {
                    Used.Foreground = Brushes.Black;
                }
            }
            Used.Content = Seting.NumberChangerMaximum(maxmemory - freememory);
            Maximum.Content = Seting.NumberChangerMaximum(maxmemory);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
            Refresh();
            DispatcherTimer dp3 = new DispatcherTimer();
            dp3.Interval = new TimeSpan(0, 5, 0);
            dp3.Tick += (sender2, args2) =>
            {
                int number = 0;
                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
                List<Value> AllInformation = new List<Value>();
                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    Value test = new Value();
                    try
                    {
                        test.Name = obj["DeviceLocator"].ToString();
                        test.Type = TypeString(Convert.ToInt32(obj["MemoryType"]));
                        test.Speed = Convert.ToInt32(obj["Speed"]);
                        test.Size = Convert.ToSingle(obj["Capacity"]);
                        test.CLI = Convert.ToByte(obj["Manufacturer"]);
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
                        valuepanel.Visibility = Visibility.Hidden;
                        informationpanel.Visibility = Visibility.Visible;
                        AddNewDataScreen("Location: ", AllInformation[index].Name);
                    }
                    else if (number == 1)
                    {
                        AddNewDataScreen("\nSize: ", Seting.NumberChangerMaximum(AllInformation[index].Size));
                    }
                    else if (number == 2)
                    {
                        AddNewDataScreen("\nType: ", AllInformation[index].Type);
                    }
                    else if (number == 3)
                    {
                        AddNewDataScreen("\nSpeed: ", AllInformation[index].Speed + " MHz");
                    }
                    else if (number == 4)
                    {
                        AddNewDataScreen("\nCLI: ", AllInformation[index].CLI + " ms");
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
                        NameP.Content = Seting.AnimatedString("RAM", number);
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
            refresh.Interval = new TimeSpan(0, 0, 0, 5);
            refresh.Tick += (sender2, args) =>
            {
                Refresh();
            };
            refresh.Start();
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
                Maximum.Foreground = Brushes.White;
                processorvaluenpanel.Foreground = Brushes.White;
                if (Used.Foreground != Brushes.Red)
                {
                    Used.Foreground = Brushes.White;
                }
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                Maximum.Foreground = Brushes.Black;
                processorvaluenpanel.Foreground = Brushes.Black;
                if (Used.Foreground != Brushes.Red)
                {
                    Used.Foreground = Brushes.Black;
                }
            }
        }
        private static string TypeString(int type)
        {
            string outValue = string.Empty;

            switch (type)
            {
                case 0x0: outValue = "Unknown"; break;
                case 0x1: outValue = "Other"; break;
                case 0x2: outValue = "DRAM"; break;
                case 0x3: outValue = "Synchronous DRAM"; break;
                case 0x4: outValue = "Cache DRAM"; break;
                case 0x5: outValue = "EDO"; break;
                case 0x6: outValue = "EDRAM"; break;
                case 0x7: outValue = "VRAM"; break;
                case 0x8: outValue = "SRAM"; break;
                case 0x9: outValue = "RAM"; break;
                case 0xa: outValue = "ROM"; break;
                case 0xb: outValue = "Flash"; break;
                case 0xc: outValue = "EEPROM"; break;
                case 0xd: outValue = "FEPROM"; break;
                case 0xe: outValue = "EPROM"; break;
                case 0xf: outValue = "CDRAM"; break;
                case 0x10: outValue = "3DRAM"; break;
                case 0x11: outValue = "SDRAM"; break;
                case 0x12: outValue = "SGRAM"; break;
                case 0x13: outValue = "RDRAM"; break;
                case 0x14: outValue = "DDR"; break;
                case 0x15: outValue = "DDR2"; break;
                case 0x16: outValue = "DDR2 FB-DIMM"; break;
                case 0x17: outValue = "Undefined 23"; break;
                case 0x18: outValue = "DDR3"; break;
                case 0x19: outValue = "FBD2"; break;
                case 0x1a: outValue = "DDR4"; break;
                default: outValue = "Undefined"; break;
            }

            return outValue;
        }
        private void AddNewDataScreen(string H1, string H2)
        {
            processorvaluenpanel.Inlines.Add(new Run(H1) { FontWeight = FontWeights.Bold });
            processorvaluenpanel.Inlines.Add(new Run(H2));
        }
    }
}
