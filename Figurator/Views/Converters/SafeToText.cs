using Avalonia.Data;
using Avalonia.Data.Converters;
using Figurator.Models;
using System;
using System.Globalization;

namespace Figurator.Views.Converters {
    internal class SafeToText: IValueConverter {

        object? item;
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value == null) return null;
            if (!targetType.IsAssignableTo(typeof(string))) throw new NotSupportedException();
            if (value is SafePoint @point) {
                item = @point;
                return @point.get;
            }
            throw new NotSupportedException();
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value == null) return null;
            if (!targetType.IsAssignableTo(typeof(SafePoint))) throw new NotSupportedException();
            if (value is not string) throw new NotImplementedException();
            var val = (string) value;
            if (item is SafePoint @point) {
                @point.set(val);
                return @point;
            }
            return null;
        }
    }
}
