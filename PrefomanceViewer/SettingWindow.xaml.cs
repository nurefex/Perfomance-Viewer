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
using System.Windows.Shapes;

namespace PrefomanceViewer
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                this.Background = Brushes.Black;
                NameP.Foreground = Brushes.White;
                CloseButton.Foreground = Brushes.White;
                CloseButton.BorderBrush = Brushes.White;
                MinimizeButton.Foreground = Brushes.White;
                MinimizeButton.BorderBrush = Brushes.White;
                Drag.Background = Brushes.DarkGray;
            }
            else
            {
                this.Background = Brushes.White;
                NameP.Foreground = Brushes.Black;
                CloseButton.Foreground = Brushes.Black;
                CloseButton.BorderBrush = Brushes.Black;
                MinimizeButton.Foreground = Brushes.Black;
                MinimizeButton.BorderBrush = Brushes.Black;
                Drag.Background = Brushes.LightGray;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
