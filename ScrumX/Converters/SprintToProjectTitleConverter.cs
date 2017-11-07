using ScrumX.API.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ScrumX.Converters
{
    class SprintToProjectTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Sprint)
            {
                if((value as Sprint).Title != null)
                {
                    return (value as Sprint).Title;
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Sprint { Title = value.ToString() };
        }
    }
}
