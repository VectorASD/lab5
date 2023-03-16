﻿using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape6_CompositeFigure: IShape {
        private static readonly PropsN[] props = new[] { PName, PCommands, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Композитка";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PName) is not string @name) return null;

            if (map.GetProp(PCommands) is not SafeGeometry @commands || !@commands.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PFillColor) is not string @fillColor) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            return new Path {
                Name = "sn|" + Utils.Base64Encode(@name) + "|" + Utils.Base64Encode(@commands.Value),
                Data = @commands.Geometry,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                Fill = new SolidColorBrush(Color.Parse(@fillColor)),
                StrokeThickness = @thickness
            };
        }



        public Dictionary<string, object?>? Export(Shape shape) {
            if (shape is not Path @path) return null;
            if (@path.Name == null || !@path.Name.StartsWith("sn|")) return null;

            var name = @path.Name.Split('|');

            return new() {
                ["name"] = Utils.Base64Decode(name[1]),
                ["path"] = Utils.Base64Decode(name[2]),
                ["stroke"] = @path.Stroke,
                ["fill"] = @path.Fill,
                ["thickness"] = (int) @path.StrokeThickness
            };
        }
        public Shape? Import(Dictionary<string, object?> data) {
            if (!data.ContainsKey("name") || data["name"] is not string @name) return null;

            if (!data.ContainsKey("path") || data["path"] is not string @path) return null;
            var commands = new SafeGeometry(@path);

            if (!data.ContainsKey("stroke") || data["stroke"] is not SolidColorBrush @color) return null;
            if (!data.ContainsKey("fill") || data["fill"] is not SolidColorBrush @fillColor) return null;
            if (!data.ContainsKey("thickness") || data["thickness"] is not short @thickness) return null;

            return new Path {
                Name = "sn|" + Utils.Base64Encode(@name) + "|" + Utils.Base64Encode(commands.Value),
                Data = commands.Geometry,
                Stroke = @color,
                Fill = @fillColor,
                StrokeThickness = @thickness
            };
        }
    }
}
