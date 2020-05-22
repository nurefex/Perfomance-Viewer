using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Windows_Tab_Manager;


namespace PrefomanceViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool SettingOpen = false;
        int NowMonitorFocus = 0;
        System.Windows.Forms.Screen[] allscreen;
        IntPtr LastFocus;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.SetValue("Performance Viewer", System.Windows.Forms.Application.ExecutablePath.ToString());
                new Thread(() =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        WindowInteropHelper wndHelper;
                        do
                        {
                            Thread.Sleep(1000);
                            wndHelper = new WindowInteropHelper(this);
                        } while (wndHelper.Handle.ToInt32() == 0);
                        int exStyle = (int)WindowAddClass.GetWindowLong(wndHelper.Handle, (int)WindowAddClass.GetWindowLongFields.GWL_EXSTYLE);
                        exStyle |= (int)WindowAddClass.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
                        WindowAddClass.SetWindowLong(wndHelper.Handle, (int)WindowAddClass.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
                    }));
                }).Start();
                Seting.LoadFile();
                LoadingWindowLocation();
                bool touch = false;
                DispatcherTimer WindowFocusManager = new DispatcherTimer();
                WindowFocusManager.Interval = new TimeSpan(0, 0, 0, 0, 100);
                WindowFocusManager.Tick += (sender2, args) =>
                  {
                      WindowInteropHelper wndHelper = new WindowInteropHelper(this);
                      IntPtr test = WindowAddClass.GetForegroundWindow();
                      if (touch == true)
                      {
                          if (wndHelper.Handle == test)
                          {
                              if (!WindowAddClass.SetForegroundWindow(LastFocus))
                              {

                              }
                          }
                          else
                          {
                              LastFocus = test;
                          }
                      }
                      else
                      {
                          if (wndHelper.Handle != test)
                          {
                              LastFocus = test;
                          }
                      }
                  };
                WindowFocusManager.Start();
                if (Seting.ColorMode == ColorMode.Dark)
                {
                    BackGroundColor.Color = Color.FromRgb(0, 0, 0);
                    this.BorderBrush = Brushes.White;
                }
                else
                {
                    BackGroundColor.Color = Color.FromRgb(255, 255, 255);
                    this.BorderBrush = Brushes.Black;
                }
                Seting.ColorModeChange += (sender1, args) =>
                  {
                      if (Seting.ColorMode == ColorMode.Dark)
                      {
                          BackGroundColor.Color = Color.FromRgb(0, 0, 0);
                      }
                      else
                      {
                          BackGroundColor.Color = Color.FromRgb(255, 255, 255);
                      }
                  };
                int maximums = Convert.ToInt32(this.Width);
                bool show = true;
                DispatcherTimer KeyHelper = new DispatcherTimer();
                KeyHelper.Interval = new TimeSpan(0, 0, 0, 0, 100);
                KeyHelper.Tick += (sender2, args) =>
                {
                    if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Alt && System.Windows.Input.Keyboard.IsKeyDown(Key.H))
                    {
                        if (show == true)
                        {
                            show = false;
                            this.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            show = true;
                            this.Visibility = Visibility.Visible;
                        }
                    }
                    else if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Alt && System.Windows.Input.Keyboard.IsKeyDown(Key.S))
                    {
                        OpenSetting();
                    }
                };
                KeyHelper.Start();
                DispatcherTimer OpenTheWindow = new DispatcherTimer();
                OpenTheWindow.Interval = new TimeSpan(0, 0, 0, 0, 900);
                OpenTheWindow.Tick += (sender2, args2) =>
                {
                    if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
                    {
                        OpenSetting();
                    }
                };
                OpenTheWindow.Start();
                DispatcherTimer MouseHelper = new DispatcherTimer();
                MouseHelper.Interval = new TimeSpan(0, 0, 0, 0, 20);
                MouseHelper.Tick += (sender2, args) =>
                  {
                      if ((System.Windows.Forms.Control.MousePosition.X >= this.Left && System.Windows.Forms.Control.MousePosition.X <= this.Left + this.Width) && (System.Windows.Forms.Control.MousePosition.Y >= this.Top && System.Windows.Forms.Control.MousePosition.Y <= this.Top + this.Height))
                      {
                          if (Seting.Lock == false && Mouse.LeftButton == MouseButtonState.Pressed)
                          {
                              this.DragMove();
                              touch = true;
                          }
                          else
                          {
                              if (touch == true)
                              {
                                  allscreen = System.Windows.Forms.Screen.AllScreens;
                                  int max = Convert.ToInt32(this.Left);
                                  int WidhtSize = 0;
                                  for (int i = 0; i < allscreen.Length; i++)
                                  {
                                      if (max <= allscreen[i].Bounds.Width)
                                      {
                                          if (max < allscreen[i].Bounds.Width / 2 - this.Width / 2)
                                          {
                                              this.Left = WidhtSize + 1;
                                              WindowReality.LocationMode = LocationMode.Left;
                                          }
                                          else
                                          {
                                              this.Left = WidhtSize + (allscreen[i].Bounds.Width - this.Width - 1);
                                              WindowReality.LocationMode = LocationMode.Right;
                                          }
                                          NowMonitorFocus = i;
                                          break;
                                      }
                                      else
                                      {
                                          max -= allscreen[i].Bounds.Width;
                                      }
                                      WidhtSize += allscreen[i].Bounds.Width;
                                  }
                              }
                          }
                      }
                      else
                      {
                          touch = false;
                          if (Seting.WindowLocation == WindowLocation.Manual)
                          {
                              allscreen = System.Windows.Forms.Screen.AllScreens;
                              int max = Convert.ToInt32(this.Left);
                              int WidhtSize = 0;
                              for (int i = 0; i < allscreen.Length; i++)
                              {
                                  if (max <= allscreen[i].Bounds.Width)
                                  {
                                      if (max < allscreen[i].Bounds.Width / 2 - this.Width / 2)
                                      {
                                          this.Left = WidhtSize + 1;
                                          WindowReality.LocationMode = LocationMode.Left;
                                      }
                                      else
                                      {
                                          this.Left = WidhtSize + (allscreen[i].Bounds.Width - this.Width - 1);
                                          WindowReality.LocationMode = LocationMode.Right;
                                      }
                                      NowMonitorFocus = i;
                                      break;
                                  }
                                  else
                                  {
                                      max -= allscreen[i].Bounds.Width;
                                  }
                                  WidhtSize += allscreen[i].Bounds.Width;
                              }

                          }
                          else if (Seting.WindowLocation == WindowLocation.FollowTheMouse)
                          {
                              allscreen = System.Windows.Forms.Screen.AllScreens;
                              int max = System.Windows.Forms.Control.MousePosition.X;
                              int WidhtSize = 0;
                              for (int i = 0; i < allscreen.Length; i++)
                              {
                                  if (max <= allscreen[i].Bounds.Width)
                                  {
                                      if (WindowReality.LocationMode == LocationMode.Left)
                                      {
                                          this.Left = WidhtSize + 1;
                                      }
                                      else
                                      {
                                          this.Left = WidhtSize + (allscreen[i].Bounds.Width - this.Width - 1);
                                      }
                                      NowMonitorFocus = i;
                                      break;
                                  }
                                  else
                                  {
                                      max -= allscreen[i].Bounds.Width;
                                  }
                                  WidhtSize += allscreen[i].Bounds.Width;
                              }
                          }
                          else if (Seting.WindowLocation == WindowLocation.FollowTheWindowFocus)
                          {
                              allscreen = System.Windows.Forms.Screen.AllScreens;
                              WindowAddClass.RECT a = new WindowAddClass.RECT();
                              WindowAddClass.GetWindowRect(WindowAddClass.GetForegroundWindow(), ref a);
                              int max = a.Left + 8;
                              int WidhtSize = 0;
                              for (int i = 0; i < allscreen.Length; i++)
                              {
                                  if (max < allscreen[i].Bounds.Width)
                                  {
                                      if (WindowReality.LocationMode == LocationMode.Left)
                                      {
                                          this.Left = WidhtSize + 1;
                                      }
                                      else
                                      {
                                          this.Left = WidhtSize + (allscreen[i].Bounds.Width - this.Width - 1);
                                      }
                                      NowMonitorFocus = i;
                                      break;
                                  }
                                  else
                                  {
                                      max -= allscreen[i].Bounds.Width;
                                  }
                                  WidhtSize += allscreen[i].Bounds.Width;
                              }
                          }
                      }
                  };
                MouseHelper.Start();
                Items.Load(MainPanel, this);
                Thickness MoveThickness = new Thickness(0, 0, 0, 0);
                DispatcherTimer WindowSettingMoving = new DispatcherTimer();
                DispatcherTimer WindowSettingMovingRevers = new DispatcherTimer();
                DispatcherTimer TopWatcher = new DispatcherTimer();
                TopWatcher.Interval = new TimeSpan(0, 0, 0, 0, 20);
                TopWatcher.Tick += (sender2, args) =>
                 {
                     if (touch == false)
                     {
                         if (this.Top + this.Height > SystemParameters.PrimaryScreenHeight)
                         {
                             this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
                         }
                         else if (this.Top < 0)
                         {
                             this.Top = 0;
                         }
                     }
                 };
                TopWatcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void OpenSetting()
        {
            if (SettingOpen == false)
            {
                SettingOpen = true;
                SettingWindow st = new SettingWindow();
                st.Show();
                st.Focus();
                st.Closed += (sender, args) =>
                  {
                      SettingOpen = false;
                  };
            }
        }

        private void LoadingWindowLocation()
        {
            WindowReality.LoadFile();
            int widthsize = 0;
            allscreen = System.Windows.Forms.Screen.AllScreens;
            try
            {
                for (int i = 1; i <= WindowReality.MonitorNumber; i++)
                {
                    if (i == WindowReality.MonitorNumber)
                    {
                        if (WindowReality.LocationMode == LocationMode.Left)
                        {
                            this.Left = widthsize + 1;
                        }
                        else
                        {
                            this.Left = widthsize + allscreen[i - 1].Bounds.Width - this.Width - 1;
                        }
                    }
                    widthsize += allscreen[i - 1].Bounds.Width;
                }
            }
            catch (Exception)
            {
                if (WindowReality.LocationMode == LocationMode.Left)
                {
                    this.Left = 0;
                }
                else
                {
                    this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 1;
                }
            }
            this.Top = WindowReality.TopWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Items.Save();
            Seting.SaveFile();
            WindowReality.SaveFile(this.Top, NowMonitorFocus + 1);
        }
    }
}