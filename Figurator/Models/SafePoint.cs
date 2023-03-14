using Avalonia;
using System;

namespace Figurator.Models {
    public class SafePoint {
        private int X, Y;
        private bool valid = true;
        public Action<object?>? Hook;
        public object? Inst;
        public SafePoint(int x, int y, Action<object?>? hook, object? inst) {
            X = x; Y = y; Hook = hook; Inst = inst;
        }

        public Point point { get => new(X, Y); }
        public bool is_valid => valid;

        public void set(string str) {
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
            if (Hook != null) Hook(Inst);
        }

        public string get { get => X + "," + Y; }
    }
}
