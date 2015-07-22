using System;
using System.Globalization;
using System.Windows.Data;

namespace Moen.KanColle.Dentan.View
{
    class BytesToStringConverter : IValueConverter
    {
        static string[] r_Suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        public object Convert(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            var rByte = System.Convert.ToInt64(rpValue);

            if (rByte == 0)
                return "0" + r_Suffix[0];

            long bytes = Math.Abs(rByte);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(rByte) * num).ToString() + r_Suffix[place];
        }

        public object ConvertBack(object rpValue, Type rpTargetType, object rpParameter, CultureInfo rpCulture)
        {
            throw new NotSupportedException();
        }
    }
}
