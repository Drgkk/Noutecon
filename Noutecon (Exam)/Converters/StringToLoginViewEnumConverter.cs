using Noutecon__Exam_.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Noutecon__Exam_.Converters
{
    public class StringToLoginViewEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LoginViewEnum loginViewEnum = (LoginViewEnum)value;
            if (loginViewEnum == LoginViewEnum.None)
            {
                return "NONE";
            }
            else if (loginViewEnum == LoginViewEnum.StudentView)
            {
                return "STUDENTVIEW";
            }
            else if (loginViewEnum == LoginViewEnum.TeacherView)
            {
                return "TEACHERVIEW";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            if (text == "NULL")
            {
                return LoginViewEnum.None;
            }
            else if (text == "STUDENTVIEW")
            {
                return LoginViewEnum.StudentView;
            }
            else if (text == "TEACHERVIEW")
            {
                return LoginViewEnum.TeacherView;
            }
            return null;
        }
    }
}
