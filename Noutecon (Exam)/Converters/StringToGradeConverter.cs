using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Noutecon__Exam_.Converters
{
    public class StringToGradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (value as string);
            return new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            string str = value as string;
            int grade;
            try
            {
                grade = int.Parse(str);
            }
            catch (Exception ex)
            {
                return null;
            }
            return grade;
        }
    }
}
