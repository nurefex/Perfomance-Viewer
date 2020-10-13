using System;
using System.Windows.Media.Imaging;

static class SystemImages
{
    public static BitmapImage BatteryBlack;
    public static BitmapImage BatteryWhite;
    public static BitmapImage ChargeBlack;
    public static BitmapImage ChargeWhite;
    public static void LoadAllSystemImages()
    {
        BatteryBlack = new BitmapImage();
        BatteryBlack.BeginInit();
        BatteryBlack.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\BatteryBlack.png");
        BatteryBlack.EndInit();
        BatteryWhite = new BitmapImage();
        BatteryWhite.BeginInit();
        BatteryWhite.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\BatteryWhite.png");
        BatteryWhite.EndInit();
        ChargeBlack = new BitmapImage();
        ChargeBlack.BeginInit();
        ChargeBlack.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\ChargeBlack.png");
        ChargeBlack.EndInit();
        ChargeWhite = new BitmapImage();
        ChargeWhite.BeginInit();
        ChargeWhite.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\ChargeWhite.png");
        ChargeWhite.EndInit();
    }
}