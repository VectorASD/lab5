using Avalonia;
using Avalonia.Controls.Shapes;
using Figurator.Models.Shapes;
using Figurator.ViewModels;
using System;
using System.Collections.Generic;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models {
    public class Mapper {
        public string shapeName = "Линия 1";

        public string shapeColor = "Blue";
        public string shapeFillColor = "Yellow";
        public int shapeThickness = 2;

        public SafeNum shapeWidth;
        public SafeNum shapeHeight;
        public SafeNum shapeHorizDiagonal;
        public SafeNum shapeVertDiagonal;

        public SafePoint shapeStartDot;
        public SafePoint shapeEndDot;
        public SafePoint shapeCenterDot;

        public SafePoints shapeDots;

        public SafeGeometry shapeCommands;

        private readonly Action<object?>? UPD;
        private readonly object? INST;

        public Mapper(Action<object?>? upd, object? inst) {
            shapeWidth = new(200, Update, this);
            shapeHeight = new(100, Update, this);
            shapeHorizDiagonal = new(100, Update, this);
            shapeVertDiagonal = new(200, Update, this);

            shapeStartDot = new(50, 50, Update, this);
            shapeEndDot = new(100, 100, Update, this);
            shapeCenterDot = new(150, 150, Update, this);

            shapeDots = new("50,50 100,100 50,100 100,50", Update, this);

            shapeCommands = new("M 10 70 l 30,30 10,10 35,0 0,-35 m 50 0 l 0,-50 10,0 35,35 m 50 0 l 0,-50 10,0 35,35z m 70 0 l 0,30 30,0 5,-35z", Update, this);
            /* 
             * Вывод: какой-то Geometry недоделанный...:
             * 1.) Geometry.Stringify нет :///////////////,
             * 2.) у Geometry.Parse нет второго параметра типа bool,
             * 3.) F x u штучки не поддерживаются :///, что позволяют добавлять всяки тени, отключать заливку и т.д...
             *      F - включает тень
             *      u - отрубает отрисовку заливки
             *      x - сбрасывает выше-перечисленные параметры
             * Возможно я GoDiagram абилку путаю с ванильной авалонией))) Ток ща заметил, что это не совсем та авалония... ;'-}
             * 4.) А, и ещё... НЕТ НОРМАЛИЗАЦИИ!!! :/// А, фух! M-параметр является глобальным! ;'-}
             */

            UPD = upd;
            INST = inst;
        }

        private static IShape[] Shapes => new IShape[] {
            new Shape1_Line(),
            new Shape2_BreakedLine(),
            new Shape3_Polygonal(),
            new Shape4_Rectangle(),
            new Shape5_Ellipse(),
            new Shape6_CompositeFigure(),
        };

        private IShape cur_shape = Shapes[0];
        private readonly Dictionary<string, Shape> shape_dict = new();
        public string? newName = null;
        public void ChangeFigure(int n) {
            cur_shape = Shapes[n];
            shapeName = GenName(cur_shape.Name);
            Update();
        }

        internal object GetProp(PropsN num) {
            return num switch {
                PName => shapeName,
                PColor => shapeColor,
                PFillColor => shapeFillColor,
                PThickness => shapeThickness,
                PWidth => shapeWidth,
                PHeight => shapeHeight,
                PHorizDiagonal => shapeHorizDiagonal,
                PVertDiagonal => shapeVertDiagonal,
                PStartDot => shapeStartDot,
                PEndDot => shapeEndDot,
                PCenterDot => shapeCenterDot,
                PDots => shapeDots,
                PCommands => shapeCommands,
                _ => 0
            };
        }

        public bool ValidInput() {
            foreach (PropsN num in cur_shape.Props)
                if (GetProp(num) is ISafe @prop && !@prop.Valid) return false;
            return true;
        }
        public bool ValidName() => !shape_dict.ContainsKey(shapeName);

        private string GenName(string prefix) {
            prefix += " ";
            int n = 1;
            while (true) {
                string res = prefix + n;
                if (!shape_dict.ContainsKey(res)) return res;
                n += 1;
            }
        }
        public Shape? Create(bool preview) {
            Shape? newy = cur_shape.Build(this);
            if (newy == null) return null;
            if (preview) return newy;

            shape_dict[shapeName] = newy;

            newName = GenName(cur_shape.Name);
            return newy;
        }

        private void Update() {
            UPD?.Invoke(INST);
        }
        private static void Update(object? me) {
            if (me != null && me is Mapper @map) @map.Update();
        }
    }
}
