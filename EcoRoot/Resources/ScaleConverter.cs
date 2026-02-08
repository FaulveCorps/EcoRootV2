using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace EcoRoot.Resources;

public class ScaleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double ratio && double.TryParse(parameter?.ToString(), out double width))
        {
            return ratio * width;
        }
        return 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
