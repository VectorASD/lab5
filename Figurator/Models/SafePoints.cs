using Avalonia;
using Avalonia.Media;
using System;
using System.Linq;

namespace Figurator.Models {
    public class SafePoints: ForcePropertyChange {
        private Points points = new();
        private bool valid = true;
        private readonly Action<object?>? hook;
        private readonly object? inst;
        public SafePoints(Action<object?>? hook, object? inst) {
            this.hook = hook; this.inst = inst;
        }

        public Points Points => points;
        public bool Valid => valid;

        public void Set(string str) {
            Points list = new();
            foreach (var p in str.Split()) {
                if (p.Length == 0) continue;

                var ss = p.Split(",");
                if (ss == null || ss.Length != 2) { valid = false; return; }

                int a, b;
                try {
                    a = int.Parse(ss[0]);
                    b = int.Parse(ss[1]);
                } catch { valid = false; return; }

                if (Math.Abs(a) > 10000 || Math.Abs(b) > 10000) { valid = false; return; }
                list.Add(new Point(a, b));
            }
            points = list;
            valid = true;
            hook?.Invoke(inst);
        }

        public string Value {
            get { valid = true; return String.Join(" ", points.Select(p => p.X + "," + p.Y)); }
            set {
                Set(value);
                UpdProperty(nameof(Color));
            }
        }

        public IBrush Color { get => valid ? Brushes.Lime : Brushes.Pink; }
    }
}
