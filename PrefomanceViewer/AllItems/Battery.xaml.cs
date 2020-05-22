using System;
using System.Collections.Generic;
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
    /// Interaction logic for Battery.xaml
    /// </summary>
    /// 
    public partial class Battery : UserControl
    {
        public static class Battery2
        {
            private static System.Windows.Forms.PowerStatus status = System.Windows.Forms.SystemInformation.PowerStatus;
            private static string LastPercent = "";
            private static string percent = "";
            public static string Percent
            {
                get
                {
                    float percent2 = status.BatteryLifePercent;
                    string percent_text = percent2.ToString("P0");
                    if (IsThere == true)
                    {
                        percent = percent_text;
                        if (percent != LastPercent)
                        {
                            LastPercent = percent;

                        }
                        return percent_text;
                    }
                    return "";
                }
            }
            public static bool IsThere
            {
                get
                {
                    if (status.BatteryChargeStatus != System.Windows.Forms.BatteryChargeStatus.NoSystemBattery)
                    {
                        return true;
                    }
                    return false;
                }
            }
            public static bool IsPower
            {
                get
                {
                    if (status.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online && IsThere == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public Battery()
        {
            if (Battery2.IsThere == true)
            {
                MinHeight = 80;
                MinWidth = 158;
                MaxHeight = 80;
                MaxWidth = 158;
                Height = 80;
                Width = 158;
                InitializeComponent();
            }
            else
            {
                DispatcherTimer dp = new DispatcherTimer();
                dp.Interval = new TimeSpan(0, 0, 0, 1);
                dp.Tick += (sender, args) =>
                  {
                      if (Battery2.IsThere == true)
                      {
                          dp.Stop();
                          MinHeight = 80;
                          MinWidth = 158;
                          MaxHeight = 80;
                          MaxWidth = 158;
                          Height = 80;
                          Width = 158;
                          InitializeComponent();
                      }
                  };
                dp.Start();
            }
        }

        double adderbattrey = 0;
        bool LastCharge = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SystemImages.LoadAllSystemImages();
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
              {
                  ColorChange();
              };
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
                        NameP.Content = Seting.AnimatedString("BATTERY", number);
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
            FocusbleChange(true);
            dp.Start();
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 0, 500);
            refresh.Tick += (sender2, args) =>
            {
                Refresh();
            };
            refresh.Start();
        }
        private void Refresh()
        {
            if (Battery2.IsThere == true)
            {
                BatteryStatusColor.Visibility = Visibility.Visible;
                BatteryItem.Visibility = Visibility.Visible;
                string batterystatus = Battery2.Percent;
                BatteryPercent.Text = batterystatus;
                int batternumber = Convert.ToInt32(batterystatus.Replace("%", ""));
                if (Battery2.IsPower == true)
                {
                    if ((0.628 * batternumber) + adderbattrey < 62.8)
                    {
                        adderbattrey += 1;
                    }
                    else
                    {
                        adderbattrey = 0;
                    }
                    BatteryStatusColor.Width = (0.628 * batternumber) + adderbattrey;
                }
                else
                {
                    BatteryStatusColor.Width = (0.628 * batternumber);
                }
                if (batternumber < 20)
                {
                    BatteryStatusColor.Background = Brushes.Red;
                }
                else if (batternumber < 50)
                {
                    BatteryStatusColor.Background = Brushes.Orange;
                }
                else if (batternumber < 80)
                {
                    BatteryStatusColor.Background = Brushes.Yellow;
                }
                else
                {
                    BatteryStatusColor.Background = Brushes.Green;
                }
                if (Battery2.IsPower != LastCharge)
                {
                    LastCharge = Battery2.IsPower;
                    if (LastCharge == true)
                    {
                        int sazmol = 0;
                        DispatcherTimer t = new DispatcherTimer();
                        t.Interval = new TimeSpan(0, 0, 0, 0, 30);
                        t.Tick += (sender1, args2) =>
                        {
                            if (sazmol < 33)
                            {
                                BatteryPercent.Margin = new Thickness(BatteryPercent.Margin.Left + 1.25, BatteryPercent.Margin.Top, BatteryPercent.Margin.Right, 0);
                                BatteryItem.Margin = new Thickness(BatteryItem.Margin.Left - 2.5, BatteryItem.Margin.Top, 0, 0);
                                ChargeIcon.Opacity += 0.03;
                                sazmol++;
                            }
                            else
                            {
                                t.Stop();
                            }
                        };
                        t.Start();
                    }
                    else
                    {
                        int szamol = 0;
                        DispatcherTimer t = new DispatcherTimer();
                        t.Interval = new TimeSpan(0, 0, 0, 0, 30);
                        t.Tick += (sender1, args2) =>
                        {
                            if (szamol < 33)
                            {
                                BatteryPercent.Margin = new Thickness(BatteryPercent.Margin.Left - 1.25, BatteryPercent.Margin.Top, BatteryPercent.Margin.Right, 0);
                                BatteryItem.Margin = new Thickness(BatteryItem.Margin.Left + 2.5, BatteryItem.Margin.Top, 0, 0);
                                ChargeIcon.Opacity -= 0.03;
                                szamol++;
                            }
                            else
                            {
                                t.Stop();
                            }
                        };
                        t.Start();
                    }
                }
            }
        }
        public void FocusbleChange(bool Value)
        {
            NameP.Focusable = Value;
            BatteryPercent.Focusable = Value;
            BatteryImage.Focusable = Value;
            ChargeIcon.Focusable = Value;
            Border.Focusable = Value;
            this.Focusable = Value;
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                BatteryPercent.Foreground = Brushes.White;
                BatteryImage.Source = SystemImages.BatteryWhite;
                ChargeIcon.Source = SystemImages.ChargeWhite;
                NameP.Foreground = Brushes.White;
            }
            else
            {
                BatteryPercent.Foreground = Brushes.Black;
                BatteryImage.Source = SystemImages.BatteryBlack;
                ChargeIcon.Source = SystemImages.ChargeBlack;
                NameP.Foreground = Brushes.Black;
            }
        }
    }
}