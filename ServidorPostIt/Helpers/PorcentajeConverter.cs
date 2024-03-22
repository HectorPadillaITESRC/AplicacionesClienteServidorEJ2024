using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServidorPostIt.Helpers
{
    public class PorcentajeConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //0 - 100
            //parameter ancho o alto
            double porcentaje = (double)value;
            double medida = double.Parse((String)parameter);

            return porcentaje * medida / 100;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
