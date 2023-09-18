using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RejestratorGlosu.Converters
{
    /// <summary>
    /// Konwerter, który przekształca ObservableCollection<Point> w PointCollection.
    /// </summary>
    public class PointCollectionConverter : IValueConverter
    {
        /// <summary>
        /// Konwertuje ObservableCollection<Point> na PointCollection.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Point> points)
            {
                PointCollection pointCollection = new PointCollection();
                foreach (var point in points)
                {
                    pointCollection.Add(point);
                }
                return pointCollection;
            }
            return null;
        }

        /// <summary>
        /// Metoda ConvertBack nie jest zaimplementowana.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
