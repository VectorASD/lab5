﻿using Avalonia.Media;
using Figurator.Models.Shapes;
using System;

namespace Figurator.Models {
    public class SafeGeometry: ForcePropertyChange, ISafe {
        private Geometry geom = new GeometryShake();
        // private string geom_str = ""; // Нет внутри Geometry метода Stringify :/// :((( ;'-{{{
        // А теперь есть)))
        private bool valid = true;
        private readonly Action<object?>? hook;
        private readonly object? inst;
        public SafeGeometry(string init, Action<object?>? hook = null, object? inst = null) {
            this.hook = hook; this.inst = inst;
            Set(init);
            if (!valid) throw new FormatException("Невалидный формат инициализации SafeGeometry: " + init);
        }
        public Geometry Geometry => geom;

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
            Geometry data;
            try {
                data = Geometry.MyParse(str);
            } catch { Upd_valid(false); return; }

            geom = data;
            Upd_valid(true);
        }

        public string Value {
            // get { Re_check(); return Geometry.Stringify(geom); } ЧЁ?!?!?!?! Нет Stringify??!!?!?! :/// А я хотел эллипс вскрыть потом :((( БОООООООЛЬ!!!!! Пэйны (из Наруто) аж позавидовали! XD
            get { Re_check(); return geom.Stringify(); }
            set {
                Set(value);
                UpdProperty(nameof(Color));
            }
        }

        public IBrush Color { get => valid ? Brushes.Lime : Brushes.Pink; }
    }
}
