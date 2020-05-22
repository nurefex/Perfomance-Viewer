using System.Windows.Forms;

namespace SystemManager.SystemInformations
{
    static class Battery
    {
        private static PowerStatus status = SystemInformation.PowerStatus;
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
                if (status.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
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
                if (status.PowerLineStatus == PowerLineStatus.Online && IsThere == true)
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
}
