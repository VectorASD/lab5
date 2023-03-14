using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Figurator.Models;
using Figurator.ViewModels;
using System;
using System.Globalization;

namespace Figurator.Views.Converters {
    internal class SafeToColor: IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            throw new Exception("Разве работает?"); // Не работает!!!!!!!!!!!!!!

            return Colors.Blue; // И даже тут не работает!!!

            var app = Application.Current;
            if (app == null) return null; // Такого просто не бывает, но надо ;'-}
            var mwvm = app.DataContext as MainWindowViewModel;
            if (mwvm == null) return null;
            mwvm.ShapeName = "YEAH!"; // Не работает!


            if (value == null) return null;
            if (value is SafePoint @point) return @point.is_valid ? Colors.White : Colors.Pink;
            throw new NotSupportedException();
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            return new NotImplementedException();
        }
    }
}
