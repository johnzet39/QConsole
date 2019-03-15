using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace QConsole.Ext
{
    static class UIHelper
    {
        //get all controls type T in container depObj 
        public static IEnumerable<T> FindVisualChildrenByType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildrenByType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        //show  tooltip for control
        public static void ShowToolTip(string message, Control control, int interval)
        {
            ToolTip tt = new ToolTip();
            tt.Content = message;
            tt.PlacementTarget = control;
            tt.Background = Brushes.MistyRose;
            tt.Placement = PlacementMode.Bottom;
            tt.StaysOpen = false;
            tt.IsOpen = true;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(interval);
            //timer.Tick += delegate //anonim method
            EventHandler handler = null;
            handler = delegate
            {
                timer.Tick -= handler;
                timer.Stop();
                tt.IsOpen = false;
                tt = null;
                timer = null;
            };
            timer.Tick += handler;
            timer.Start();
        }

    }

    
}
