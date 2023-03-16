using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Figurator.Models.Shapes;
using Figurator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        public readonly ObservableCollection<ShapeListBoxItem> shapes = new();

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

        private static IShape[] Shapers => new IShape[] {
            new Shape1_Line(),
            new Shape2_BreakedLine(),
            new Shape3_Polygonal(),
            new Shape4_Rectangle(),
            new Shape5_Ellipse(),
            new Shape6_CompositeFigure(),
        };
        private static Dictionary<string, IShape> TShapers => new(Shapers.Select(shaper => new KeyValuePair<string, IShape>(shaper.Name, shaper)));

        private IShape cur_shaper = Shapers[0];
        private readonly Dictionary<string, Shape> shape_dict = new();
        public string? newName = null;
        public void ChangeFigure(int n) {
            cur_shaper = Shapers[n];
            shapeName = GenName(cur_shaper.Name);
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
            foreach (PropsN num in cur_shaper.Props)
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
            Shape? newy = cur_shaper.Build(this);
            if (newy == null) return null;
            if (preview) return newy;

            shape_dict[shapeName] = newy;
            shapes.Add(new ShapeListBoxItem(shapeName, this));

            newName = GenName(cur_shaper.Name);
            return newy;
        }

        internal void Remove(ShapeListBoxItem item) {
            var Name = item.Name;
            if (!shape_dict.ContainsKey(Name)) return;

            var shape = shape_dict[Name];
            if (shape == null || shape.Parent is not Canvas @c) return;

            @c.Children.Remove(shape);
            shapes.Remove(item);
            shape_dict.Remove(Name);

            newName = GenName(cur_shaper.Name);
            Update();
        }

        public void Clear() {
            foreach (var item in shape_dict) {
                var shape = item.Value;
                if (shape == null || shape.Parent is not Canvas @c) continue;
                @c.Children.Clear();
            }
            shapes.Clear();
            shape_dict.Clear();

            newName = GenName(cur_shaper.Name);
            Update();
        }

        public void Export(bool is_xml) {
            List<object> data = new();
            foreach (var item in shape_dict) {
                var shape = item.Value;
                // Log.Write("shape: " + shape);
                bool R = true;
                foreach (var shaper in Shapers) {
                    var res = shaper.Export(shape);
                    // Log.Write("  res: " + res);
                    if (res != null) {
                        res["type"] = shaper.Name;
                        data.Add(res);
                        R = false;
                        break;
                    }
                }
                if (R) Log.Write("Потеряна одна из фигур при экспортировании :/");
            }
            var json = Utils.Obj2json(data);
            //var xml = Utils.Json2xml(json);

            Log.Write("J: " + json);
            //Log.Write("J: " + Utils.Xml2json(xml));

            File.WriteAllText("../../../Export.json", json);
        }

        public Shape[]? Import(bool is_xml) {
            if (!File.Exists("../../../Export.json")) { Log.Write("Export.json не обнаружен"); return null; }
            var data = File.ReadAllText("../../../Export.json");
            var json = Utils.Json2obj(data);
            if (json is not List<object?> @list) { Log.Write("В начале Export.json не список"); return null; }

            List<Shape> res = new();
            shape_dict.Clear();
            shapes.Clear();

            foreach (object? item in @list) {
                if (item is not Dictionary<string, object?> @dict) { Log.Write("Одна из фигур при импорте - не словарь"); continue; }
                // Log.Write("D: " + @dict); // Работает!!!

                if (!@dict.ContainsKey("type") || @dict["type"] is not string @type) { Log.Write("Нет поля type, либо оно - не строка"); continue; }
                if (!@dict.ContainsKey("name") || @dict["name"] is not string @shapeName) { Log.Write("Нет поля name, либо оно - не строка"); continue; }
                if (!TShapers.ContainsKey(@type)) { Log.Write("Фигуратор " + @type + " не обнаружен :/"); continue; }

                var shaper = TShapers[@type];
                var newy = shaper.Import(@dict);
                if (newy == null) { Log.Write("Не получилось собрть фигуру " + Utils.Obj2json(@dict)); continue; }

                // Log.Write("N: " + @type);
                shape_dict[shapeName] = newy;
                shapes.Add(new ShapeListBoxItem(shapeName, this));
                res.Add(newy);
            }
            
            newName = GenName(cur_shaper.Name);
            return res.ToArray();
        }

        private void Update() {
            UPD?.Invoke(INST);
        }
        private static void Update(object? me) {
            if (me != null && me is Mapper @map) @map.Update();
        }
    }
}
