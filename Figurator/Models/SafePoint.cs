﻿using Avalonia;
using Avalonia.Media;
using System;

namespace Figurator.Models {
    public class SafePoint: ForcePropertyChange {
        private int X, Y;
        private bool valid = true;
        private readonly Action<object?>? hook;
        private readonly object? inst;
        public SafePoint(int x, int y, Action<object?>? hook, object? inst) {
            X = x; Y = y; this.hook = hook; this.inst = inst;
        }

        public Point Point { get => new(X, Y); }
        public bool Valid => valid;

        public void Set(string str) {
            var ss = str.Split(",");
            if (ss == null || ss.Length != 2) { valid = false; return; }

            int a, b;
            try {
                a = int.Parse(ss[0]);
                b = int.Parse(ss[1]);
            } catch { valid = false; return; }

            if (Math.Abs(a) > 10000 || Math.Abs(b) > 10000) { valid = false; return; }

            X = a; Y = b;
            valid = true;
            hook?.Invoke(inst);
        }

        /*private void check([CallerMemberName] string? propertyName = null) {
            //throw new Exception("LOL: " + propertyName + "|" + valid);
            // UpdProperty("ShapeStartDot"); // Не подходит, ХОТЬ И ДОЛЖНО БЫЛО!!!
            // UpdProperty(propertyName); // propertyName == "Value" Подходит, но только для элементов, юзающих в биндинге .Value
            UpdProperty("Color");
            // {Binding ShapeStartDot, Converter={StaticResource S2C_converter}} оказался слишком упёртым :/
            // В таком случае я ВЫНУЖДЕН отказать от конвертеров из-за невозможности подать событие в него :/
            // Вместо этого будет использоваться {Binding ShapeStartDot.Color} т.к. он ХОТЯБЫ РАБОТАЕТ...
        }*/

        public string Value {
            get { valid = true; return X + "," + Y; }
            set {
                Set(value);
                // string res = ""; // Обычно для этого юзают StringBuilder, но это отладочный образец, так что не важно.
                // foreach(var frame in new StackTrace().GetFrames()) res += frame.GetMethod().Name + "\n";
                // throw new Exception("LOL: " + res); // В целом это позволяет вытащить слово "set_Value", но оказывается есть nameof(Value), что == "Value"
                // check();
                UpdProperty(nameof(Color));
                //   Тоже не рабочий вариант в случае изменения поля Конечная точка... Цвет не меняется, конвертер предал.
                // UpdProperty(); // Равносильно UpdProperty(nameof(Value))
                // UpdProperty("ShapeEndDot");
            }
        }

        public IBrush Color { get => valid ? Brushes.Lime : Brushes.Pink; }
    }
}
