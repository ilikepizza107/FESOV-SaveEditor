using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FESOVSE.Extension
{
    public static class WindowExtension
    {

        /* Code snippet from https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type */
        /* Used to get all controls specified from the xaml */
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
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

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            if (parentObject is T parent)
            {
                return parent;
            }

            return FindParent<T>(parentObject);
        }
    }
}
