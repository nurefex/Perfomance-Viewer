using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PrefomanceViewer
{
    static class Items
    {
        public class ItemCreator
        {
            private bool lastvalue;
            private UserControl control;
            public ItemCreator(UserControl panel, bool location)
            {
                control = panel;
                lastvalue = !(location);
            }
            public void Refresh(bool LocationValue)
            {
                if (lastvalue != LocationValue)
                {
                    lastvalue = LocationValue;
                    if (LocationValue == true)
                    {
                        try
                        {
                            NoViewer.Children.Remove(control);
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            Viewer.Children.Add(control);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            Viewer.Children.Remove(control);
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            NoViewer.Children.Add(control);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
        private static List<ItemCreator> AllItem = new List<ItemCreator>();
        private static List<bool> AllOnOrOff = new List<bool>();
        private static WrapPanel Viewer;
        public static WrapPanel ViewerPanel
        {
            set
            {
                Viewer = value;
            }
        }
        private static WrapPanel NoViewer;
        public static WrapPanel NoViewerPanel
        {
            set
            {
                NoViewer = value;
            }
        }
        private static DispatcherTimer ViewWatcher;
        static Items()
        {
            ViewWatcher = new DispatcherTimer();
            ViewWatcher.Interval = new TimeSpan(0, 0, 0, 1);
            AllItem.Add(new ItemCreator(new AllItems.Time(), Seting.TimeOn));
            AllItem.Add(new ItemCreator(new AllItems.Processor(),Seting.ProcessorOn));
            AllItem.Add(new ItemCreator(new AllItems.Memory(),Seting.RamOn));
            AllItem.Add(new ItemCreator(new AllItems.Disk(),Seting.DiskOn));
            AllItem.Add(new ItemCreator(new AllItems.Network(),Seting.NetworkOn));
            AllItem.Add(new ItemCreator(new AllItems.Battery(),Seting.BatteryOn));
            AllItem.Add(new ItemCreator(new AllItems.NetworkInformation(),Seting.NetworkInformation));
            ViewWatcher.Tick += (sender, args) =>
              {
                  ListAllOnOrOff();
                  for (int i = 0; i < AllItem.Count; i++)
                  {
                      AllItem[i].Refresh(AllOnOrOff[i]);
                  }
              };
            ViewWatcher.Start();
        }
        private static void ListAllOnOrOff()
        {
            AllOnOrOff.Add(Seting.TimeOn);
            AllOnOrOff.Add(Seting.ProcessorOn);
            AllOnOrOff.Add(Seting.RamOn);
            AllOnOrOff.Add(Seting.DiskOn);
            AllOnOrOff.Add(Seting.NetworkOn);
            AllOnOrOff.Add(Seting.BatteryOn);
            AllOnOrOff.Add(Seting.NetworkInformation);
        }
    }
}