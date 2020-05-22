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
    /// Interaction logic for Time.xaml
    /// </summary>
    public partial class Time : UserControl
    {
        public Time()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorChange();
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
                        NameP.Content = Seting.AnimatedString("TIME", number);
                        number += new Random().Next(0, 4) / 3;
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
            DispatcherTimer refresh = new DispatcherTimer();
            refresh.Interval = new TimeSpan(0, 0, 0, 1);
            refresh.Tick += (sender2, args) =>
            {
                Refresh();
            };
            refresh.Start();
        }
        private void Refresh()
        {
            DateL.Content = DateTime.Now.ToString("yyyy.MM.dd.");
            TimeL.Content = DateTime.Now.ToString("HH:mm");
            TimeSecL.Content = DateTime.Now.ToString("ss");
            DayOfWeekLabel.Content = DateTime.Now.DayOfWeek.ToString();
        }
        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                NameP.Foreground = Brushes.White;
                TimeL.Foreground = Brushes.White;
                DateL.Foreground = Brushes.White;
                TimeSecL.Foreground = Brushes.White;
                DayOfWeekLabel.Foreground = Brushes.White;
            }
            else
            {
                NameP.Foreground = Brushes.Black;
                TimeL.Foreground = Brushes.Black;
                DateL.Foreground = Brushes.Black;
                TimeSecL.Foreground = Brushes.Black;
                DayOfWeekLabel.Foreground = Brushes.Black;
            }
        }
    }
}