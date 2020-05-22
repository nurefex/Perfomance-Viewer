using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PrefomanceViewer
{
    public enum ColorMode
    {
        Dark,
        Light
    }
    public enum LocationMode
    {
        Right,
        Left
    }
    public enum WindowLocation
    {
        Manual,
        FollowTheMouse,
        FollowTheWindowFocus
    }
    static class Items
    {
        private static Grid Viewer;
        private static Window window;
        public static void Save()
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\ItemValue.data", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("ProcessorValue:" + Processor.Data.Value);
            sw.WriteLine("RamValue:" + Ram.Data.Value);
            sw.WriteLine("DriveValue:" + Drive.Data.Value);
            sw.WriteLine("NetwokrValue:" + Netwwork.Data.Value);
            sw.WriteLine("BatteryValue:" + Battery.Data.Value);
            sw.WriteLine("NetworkInformationValue:" + NetworkInformation.Data.Value);
            sw.WriteLine("TimeValue:" + Time.Data.Value);
            sw.Close();
            fs.Close();
        }
        public static void Load(Grid ViewverAdd, Window windowAdd)
        {
            window = windowAdd;
            Viewer = ViewverAdd;
            try
            {
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\ItemValue.data", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                do
                {
                    string[] test = sr.ReadLine().Split(':');
                    if (test[0] == "ProcessorValue")
                    {
                        Processor.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "RamValue")
                    {
                        Ram.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "DriveValue")
                    {
                        Drive.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "NetwokrValue")
                    {
                        Netwwork.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "BatteryValue")
                    {
                        Battery.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "NetworkInformationValue")
                    {
                        NetworkInformation.StartValue = Convert.ToInt32(test[1]);
                    }
                    else if (test[0] == "TimeValue")
                    {
                        Time.StartValue = Convert.ToInt32(test[1]);
                    }
                } while (sr.Peek() != -1);
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {
                Time.StartValue = 1;
                Processor.StartValue = 2;
                Ram.StartValue = 3;
                Drive.StartValue = 4;
                Netwwork.StartValue = 5;
                Battery.StartValue = 6;
                NetworkInformation.StartValue = -1;
            }
            RefreshItemLocation();
        }
        private static List<DefaultClass> allitem = new List<DefaultClass>();
        private static void RefreshItemLocation()
        {
            Viewer.Children.Clear();
            double WidhtSize = 0;
            double TopSize = 0;
            int max = 0;
            for (int i = 0; i < allitem.Count; i++)
            {
                if (allitem[i].Value > max)
                {
                    max = allitem[i].Value;
                }
            }
            for (int i = 0; i <= max; i++)
            {
                for (int j = 0; j < allitem.Count; j++)
                {
                    if (allitem[j].Value == i)
                    {
                        if (!Double.IsInfinity(allitem[j].TheItem.MaxWidth))
                        {
                            if (allitem[j].TheItem.MaxWidth > 80 && WidhtSize > 0)
                            {
                                TopSize += 78;
                                WidhtSize = 0;
                                Viewer.Height = TopSize + 2;
                            }
                            allitem[j].TheItem.Margin = new System.Windows.Thickness(WidhtSize, TopSize, 0, 0);
                            try
                            {
                                Viewer.Children.Add(allitem[j].TheItem);
                            }
                            catch (Exception)
                            {

                            }
                            WidhtSize += allitem[j].TheItem.MaxWidth - 2;
                        }
                        if (WidhtSize > 80)
                        {
                            TopSize += 78;
                            Viewer.Height = TopSize + 2;
                            WidhtSize = 0;
                        }
                        break;
                    }
                }
            }
        }
        class DefaultClass
        {
            private UserControl theitem;
            public UserControl TheItem
            {
                set
                {
                    theitem = value;
                }
                get
                {
                    return theitem;
                }
            }
            private int value;
            public int Value
            {
                set
                {
                    if (this.value != value)
                    {
                        if (this.value > 0)
                        {
                            for (int i = 0; i < allitem.Count; i++)
                            {
                                if (allitem[i].value >= value)
                                {
                                    allitem[i].value++;
                                }
                            }
                        }
                        else if (value < 0)
                        {
                            for (int i = 0; i < allitem.Count; i++)
                            {
                                if (allitem[i].value <= value)
                                {
                                    allitem[i].value--;
                                }
                            }
                        }
                        this.value = value;
                        RefreshItemLocation();
                    }
                }
                get
                {
                    return this.value;
                }
            }
            double xmouse = 0;
            double ymouse = 0;
            bool isInDrag = false;
            private TranslateTransform transform = new TranslateTransform();
            public DefaultClass(UserControl control, int itemvalue)
            {
                theitem = control;
                theitem.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                theitem.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                value = itemvalue;
                /*theitem.MouseRightButtonDown += (sender, e) =>
                  {
                      xmouse= e.GetPosition((UserControl)sender).X;
                      ymouse = e.GetPosition((UserControl)sender).Y;
                      //MessageBox.Show(xmouse+":"+ymouse);
                      isInDrag = true;
                      e.Handled = true;
                  };
                theitem.MouseMove += (sender, e) =>
                 {
                     if (isInDrag)
                     {
                         //MessageBox.Show(String.Format("{0}:{1}", System.Windows.Forms.Control.MousePosition.X - window.Left, System.Windows.Forms.Control.MousePosition.Y));
                         theitem.Margin = new Thickness(System.Windows.Forms.Control.MousePosition.X - window.Left - xmouse/2, System.Windows.Forms.Control.MousePosition.Y - window.Width - ymouse/2, 0, 0);
                     }
                 };
                theitem.MouseRightButtonUp += (sender, e) =>
                  {
                      if (isInDrag)
                      {
                          isInDrag = false;
                          e.Handled = true;
                      }
                  };*/
            }
            private void RefreshLocationPreview()
            {
                int max = 0;
                for (int i = 0; i < allitem.Count; i++)
                {
                    if (allitem[i].Value > max)
                    {
                        max = allitem[i].Value;
                    }
                }
                for (int i = 1; i <= max; i++)
                {
                    for (int j = 0; j < allitem.Count; j++)
                    {
                        if (allitem[j].Value == i)
                        {
                            if (allitem[j].theitem.Margin.Top <= theitem.Margin.Top && allitem[j].theitem.Margin.Top + allitem[j].theitem.MaxHeight >= theitem.Margin.Top && allitem[j].theitem.Margin.Left <= theitem.Margin.Left && allitem[j].theitem.Margin.Left + allitem[j].theitem.MaxWidth >= theitem.Margin.Left)
                            {
                                this.Value = i;
                                return;
                            }
                        }
                    }
                }
            }
        }
        static class Processor
        {
            public static DefaultClass Data;
            private static AllItems.Processor item = new AllItems.Processor();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
        static class Ram
        {
            public static DefaultClass Data;
            private static AllItems.Memory item = new AllItems.Memory();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
        static class Drive
        {
            public static DefaultClass Data;
            private static AllItems.Drive item = new AllItems.Drive();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
        static class Netwwork
        {
            public static DefaultClass Data;
            private static AllItems.Network item = new AllItems.Network();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
        static class Battery
        {
            public static DefaultClass Data;
            private static AllItems.Battery item = new AllItems.Battery();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                    if (AllItems.Battery.Battery2.IsThere)
                    {
                        Data.TheItem.MaxWidth = 158;
                        Data.TheItem.MaxHeight = 80;
                    }
                }
            }
        }
        static class NetworkInformation
        {
            public static DefaultClass Data;
            private static AllItems.NetworkInformation item = new AllItems.NetworkInformation();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
        static class Time
        {
            public static DefaultClass Data;
            private static AllItems.Time item = new AllItems.Time();
            public static int StartValue
            {
                set
                {
                    Data = new DefaultClass(item, value);
                    allitem.Add(Data);
                }
            }
        }
    }
    static class Seting
    {
        private static WindowLocation windowlocation = WindowLocation.Manual;
        public static WindowLocation WindowLocation
        {
            set
            {
                windowlocation = value;
            }
            get
            {
                return windowlocation;
            }
        }
        public static string AnimatedString(string String2, int number)
        {
            StringBuilder sb = new StringBuilder(String2);
            Random rnd = new Random();
            for (int i = number; i < sb.Length; i++)
            {
                char test = (char)rnd.Next(0, 256);
                if (test != '\n' && test != '\t')
                {
                    sb[i] = test;
                }
                else
                {
                    i--;
                }
            }
            return sb.ToString();
        }
        private static bool animatedstringsetting = true;
        public static bool AnimatedStringSetting
        {
            set
            {
                animatedstringsetting = value;
            }
            get
            {
                return animatedstringsetting;
            }
        }
        private static bool lockt = false;
        public static bool Lock
        {
            get
            {
                return lockt;
            }
            set
            {
                lockt = value;
            }
        }
        private static ColorMode colormode = ColorMode.Light;
        public static ColorMode ColorMode
        {
            set
            {
                if (colormode != value)
                {
                    colormode = value;
                    ColorModeChange(null, EventArgs.Empty);
                }
            }
            get
            {
                return colormode;
            }
        }
        public static event EventHandler ColorModeChange;
        public static string NumberChangerMaximum(float bytes)
        {
            string[] allytypes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int indesx = 0;
            while (bytes > 1024)
            {
                bytes = bytes / 1024;
                indesx++;
            }
            if (bytes < 100)
            {
                return Convert.ToString(Math.Round(bytes, 1)) + " " + allytypes[indesx];
            }
            else
            {
                return Convert.ToString(Math.Round(bytes, 0)) + " " + allytypes[indesx];
            }
        }
        public static void LoadFile()
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.data", FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            do
            {
                try
                {
                    string[] test = sr.ReadLine().Split(':');
                    if (test[0] == "ColorMode")
                    {
                        Enum.TryParse(test[1], out colormode);
                    }
                    else if (test[0] == "AniatedString")
                    {
                        animatedstringsetting = Convert.ToBoolean(test[0]);
                    }
                    else if (test[0] == "WindowLocation")
                    {
                        Enum.TryParse(test[1], out windowlocation);
                    }
                    else if (test[0] == "Lock")
                    {
                        lockt = Convert.ToBoolean(test[1]);
                    }
                }
                catch (Exception)
                {

                }
            } while (sr.Peek() != -1);
            sr.Close();
            fs.Close();
        }
        public static void SaveFile()
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.data", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("ColorMode:" + colormode);
            sw.WriteLine("AniatedString:" + animatedstringsetting);
            sw.WriteLine("WindowLocation:" + windowlocation);
            sw.WriteLine("Lock:" + lockt);
            sw.Close();
            fs.Close();
        }
    }
    public static class WindowReality
    {
        private static LocationMode location = LocationMode.Right;
        public static LocationMode LocationMode
        {
            set
            {
                location = value;
            }
            get
            {
                return location;
            }
        }
        private static int monitornumber = 1;
        public static int MonitorNumber
        {
            set
            {
                monitornumber = value;
            }
            get
            {
                return monitornumber;
            }
        }
        private static double topwindow = 0;
        public static double TopWindow
        {
            get
            {
                return topwindow;
            }
        }
        public static void LoadFile()
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\RealityLoading.data", FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            do
            {
                try
                {
                    string[] test = sr.ReadLine().Split(':');
                    if (test[0] == "LocationMode")
                    {
                        Enum.TryParse(test[1], out location);
                    }
                    else if (test[0] == "TopLocation")
                    {
                        topwindow = Convert.ToDouble(test[1]);
                    }
                    else if (test[0] == "MonitorNumber")
                    {
                        monitornumber = Convert.ToInt32(test[1]);
                    }
                }
                catch (Exception)
                {

                }
            } while (sr.Peek() != -1);
            sr.Close();
            fs.Close();
        }
        public static void SaveFile(double TopWindowLoc, int monitornumber)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\RealityLoading.data", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("LocationMode:" + location);
            sw.WriteLine("TopLocation:" + TopWindowLoc);
            sw.WriteLine("MonitorNumber:" + monitornumber);
            sw.Close();
            fs.Close();
        }
    }
}