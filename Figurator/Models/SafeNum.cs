﻿using Avalonia;
using Avalonia.Media;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Figurator.Models {
    public class SafeNum: ForcePropertyChange, ISafe {
        private int num;
        private bool valid = true;
        private readonly Action<object?>? hook;
        private readonly object? inst;
        public SafeNum(int num, Action<object?>? hook, object? inst) {
            this.num = num; this.hook = hook; this.inst = inst;
        }
        public int Num => num;

        private void Upd_valid(bool v) {
            valid = v;
            hook?.Invoke(inst);
        }
        private void Re_check() {
            if (!valid) {
                valid = true;
                //hook?.Invoke(inst);
            }
        }

        /*
         * ISafe-часть:
         */

        public bool Valid => valid;

        public void Set(string str) {
            int a;
            try {
                a = int.Parse(str);
            } catch { Upd_valid(false); return; }

            if (Math.Abs(a) > 10000) { Upd_valid(false); return; }

            num = a;
            Upd_valid(true);
        }

        public string Value {
            get { Re_check(); return num.ToString(); }
            set {
                Set(value);
                UpdProperty(nameof(Color));
            }
        }

        public IBrush Color { get => valid ? Brushes.Lime : Brushes.Pink; }
    }
}
