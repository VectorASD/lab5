﻿using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;

namespace Figurator.Models.Shapes {
    public enum PropsN {
        PName, PColor, PFillColor, PThickness,
        PWidth, PHeight, PHorizDiagonal, PVertDiagonal,
        PStartDot, PEndDot, PCenterDot, PDots,
        PCommands
    }
    internal interface IShape {
        public PropsN[] Props { get; }
        public string Name { get; }
        public Shape? Build(Mapper map);
        public Dictionary<string, object?>? Export(Shape shape);
        public Shape? Import(Dictionary<string, object?> data);
    }
}
