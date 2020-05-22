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

namespace PrefomanceViewer
{
    /// <summary>
    /// Interaction logic for SettingPanel.xaml
    /// </summary>
    public partial class SettingPanel : UserControl
    {
        public SettingPanel()
        {
            InitializeComponent();
        }

        private void DarkRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Seting.ColorMode = ColorMode.Dark;
        }

        private void LightRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Seting.ColorMode = ColorMode.Light;
        }

        private void WrapPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Seting.WindowLocation == WindowLocation.Manual)
            {
                WindowManual.IsChecked = true;
            }
            else if (Seting.WindowLocation == WindowLocation.FollowTheMouse)
            {
                followsthemouse.IsChecked = true;
            }
            else if (Seting.WindowLocation == WindowLocation.FollowTheWindowFocus)
            {
                followsthewindowfocus.IsChecked = true;
            }
            YesAnimated.IsChecked = Seting.AnimatedStringSetting;
            NoAnimated.IsChecked = !Seting.AnimatedStringSetting;
            YesWindowLock.IsChecked = Seting.Lock;
            NoWindowLock.IsChecked = !Seting.Lock;
            ColorChange();
            Seting.ColorModeChange += (sender2, args) =>
            {
                ColorChange();
            };
        }

        private void ColorChange()
        {
            if (Seting.ColorMode == ColorMode.Dark)
            {
                LightRadioButton.Foreground = Brushes.White;
                DarkRadioButton.Foreground = Brushes.White;
                ColorTitle.Foreground = Brushes.White;
                YesAnimated.Foreground = Brushes.White;
                NoAnimated.Foreground = Brushes.White;
                AnimatedTitle.Foreground = Brushes.White;
                WindowLocationTitle.Foreground = Brushes.White;
                WindowManual.Foreground = Brushes.White;
                followsthemouse.Foreground = Brushes.White;
                followsthewindowfocus.Foreground = Brushes.White;
                WindowLockTitle.Foreground = Brushes.White;
                YesWindowLock.Foreground = Brushes.White;
                NoWindowLock.Foreground = Brushes.White;
                DarkRadioButton.IsChecked = true;
            }
            else
            {
                ColorTitle.Foreground = Brushes.Black;
                LightRadioButton.Foreground = Brushes.Black;
                DarkRadioButton.Foreground = Brushes.Black;
                YesAnimated.Foreground = Brushes.Black;
                NoAnimated.Foreground = Brushes.Black;
                AnimatedTitle.Foreground = Brushes.Black;
                WindowLocationTitle.Foreground = Brushes.Black;
                WindowManual.Foreground = Brushes.Black;
                followsthemouse.Foreground = Brushes.Black;
                followsthewindowfocus.Foreground = Brushes.Black;
                WindowLockTitle.Foreground = Brushes.Black;
                YesWindowLock.Foreground = Brushes.Black;
                NoWindowLock.Foreground = Brushes.Black;
                LightRadioButton.IsChecked = true;
            }
        }

        private void NoAnimated_Checked(object sender, RoutedEventArgs e)
        {
            Seting.AnimatedStringSetting = false;
        }

        private void YesAnimated_Checked(object sender, RoutedEventArgs e)
        {
            Seting.AnimatedStringSetting = true;
        }

        private void WindowManual_Checked(object sender, RoutedEventArgs e)
        {
            Seting.WindowLocation = WindowLocation.Manual;
        }

        private void followsthemouse_Checked(object sender, RoutedEventArgs e)
        {
            Seting.WindowLocation = WindowLocation.FollowTheMouse;
        }

        private void followsthewindowfocus_Checked(object sender, RoutedEventArgs e)
        {
            Seting.WindowLocation = WindowLocation.FollowTheWindowFocus;
        }

        private void YesWindowLock_Checked(object sender, RoutedEventArgs e)
        {
            Seting.Lock = true;
        }

        private void NoWindowLock_Checked(object sender, RoutedEventArgs e)
        {
            Seting.Lock = false;
        }
    }
}
