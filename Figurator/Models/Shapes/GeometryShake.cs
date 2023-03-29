using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using System;

namespace Figurator.Models.Shapes {
    public static class GeometryDop {
        public static string Stringify(this Geometry geom) {
            if (geom is not GeometryShake @shake) throw new Exception("Geometry не является предком GeometryShake");
            return @shake.saved_path;
        }
        public static Geometry MyParse(this Geometry _, string s) => GeometryShake.Parse(s);
    }

    public class GeometryShake: Geometry {
        // Т.к. в StreamGeometry нельзя переопределить статический Parse метод,
        // то приходится идти на такие крайности - полная копия StreamGeometry)))

        // Всё ради добавления Stringify, что сохраняет поданные значения в Parse...

        // Теперь у всех Path фигур Data иметь тип GeometryShake вместо StreamGeometry,
        // при том ничего не сломается вообще! ;'-}}}

        IStreamGeometryImpl? _impl;
        public string saved_path;

        public GeometryShake(string path = "") { saved_path = path; }

        private GeometryShake(string path, IStreamGeometryImpl impl) { saved_path = path; _impl = impl; }

        public static new GeometryShake Parse(string s) {
            // Эту штуку пришлось сделать виртуальной (не статической), иначе я опять же не смогу ничего нормально сохранять!!!!!
            // А хотя... у меня же есть переменная geometryShake, АХАХАХАХ!!! Туда же и буду пихать :S

            var geometryShake = new GeometryShake(s);

            using (var context = geometryShake.Open())
            using (var parser = new PathMarkupParser(context)) parser.Parse(s);

            return geometryShake;
        }

        public override Geometry Clone() {
            return new GeometryShake(saved_path, ((IStreamGeometryImpl) PlatformImpl).Clone());
        }

        public StreamGeometryContext Open() {
            return new StreamGeometryContext(((IStreamGeometryImpl) PlatformImpl).Open());
        }

        protected override IGeometryImpl CreateDefiningGeometry() {
            if (_impl == null) {
                var factory = AvaloniaLocator.Current.GetService<IPlatformRenderInterface>() ?? throw new Exception("Factory not found");
                _impl = factory.CreateStreamGeometry();
                // Вот эта штука и хранит каким-то образом поданные примитивные команды с парсера геометрического пути.
            }
            return _impl;
        }
    }
}
