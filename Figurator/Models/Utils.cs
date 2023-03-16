using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Media;
using System.Text.Json;
using Figurator.ViewModels;

namespace Figurator.Models {
    public class Utils {
        // By VectorASD         Всё это моих рук дела! ;'-}

        public static string Base64Encode(string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }



        public static string JsonEscape(string str) {
            StringBuilder sb = new();
            foreach (char i in str) {
                sb.Append(i switch {
                    '"' => "\\\"",
                    '\\' => "\\\\",
                    '$' => "\\$", // Чисто по моей части ;'-}
                     _ => i
                });
            }
            return sb.ToString();
        }
        public static string Obj2json(object? obj) { // Велосипед ради поддержки своей сериализации классов по типу Point, SolidColorBrush и т.д.
            if (obj == null) return "null";
            if (obj is string @str) return '"' + JsonEscape(str) + '"';
            if (obj is bool @bool) return @bool ? "true" : "false";
            if (obj is short @short) return @short.ToString();
            if (obj is int @int) return @int.ToString();
            if (obj is long @long) return @long.ToString();
            if (obj is float @float) return @float.ToString();
            if (obj is double @double) return @double.ToString();

            if (obj is Point @point) return "\"$p$" + (int) @point.X + "," + (int) @point.Y + '"';
            if (obj is Points @points) return "\"$P$" + string.Join("|", @points.Select(p => (int) p.X + "," + (int) p.Y)) + '"';
            if (obj is SolidColorBrush @color) return "\"$C$" + @color.Color + '"';
            if (obj is Thickness @thickness) return "\"$T$" + @thickness.Left + "," + @thickness.Top + "," + @thickness.Right + "," + @thickness.Bottom + '"';

            if (obj is List<object?> @list) {
                StringBuilder sb = new();
                sb.Append('[');
                foreach (object? item in @list) {
                    if (sb.Length > 1) sb.Append(", ");
                    sb.Append(Obj2json(item));
                }
                sb.Append(']');
                return sb.ToString();
            }
            if (obj is Dictionary<string, object?> @dict) {
                StringBuilder sb = new();
                sb.Append('{');
                foreach (var entry in @dict) {
                    if (sb.Length > 1) sb.Append(", ");
                    sb.Append(Obj2json(entry.Key));
                    sb.Append(": ");
                    sb.Append(Obj2json(entry.Value));
                }
                sb.Append('}');
                return sb.ToString();
            }

            return "(" + obj.GetType() + " ???)";
        }

        private static object JsonHandler(string str) {
            if (str.Length < 3 || str[0] != '$' || str[2] != '$') return str;
            string data = str[3..];
            string[] thick = str[1] == 'T' ? data.Split(',') : System.Array.Empty<string>();
            return str[1] switch {
                'p' => new SafePoint(data).Point,
                'P' => new SafePoints(data.Replace('|', ' ')).Points,
                'C' => new SolidColorBrush(Color.Parse(data)),
                'T' => new Thickness(double.Parse(thick[0]), double.Parse(thick[1]), double.Parse(thick[2]), double.Parse(thick[3])),
                _ => str,
            };
        }
        private static object? JsonHandler(object? obj) {
            if (obj == null) return null;

            if (obj is List<object?> @list) return @list.Select(JsonHandler).ToList();
            if (obj is Dictionary<string, object?> @dict) return @dict.Select(pair => new KeyValuePair<string, object?>(pair.Key, JsonHandler(pair.Value)));
            if (obj is JsonElement @item) {
                switch (@item.ValueKind) {
                case JsonValueKind.Undefined: return null;
                case JsonValueKind.Object:
                    Dictionary<string, object?> res = new();
                    foreach (var el in @item.EnumerateObject()) res[el.Name] = JsonHandler(el.Value);
                    return res;
                case JsonValueKind.Array: // Неиспытано ещё
                    List<string> res2 = new();
                    foreach (var el in @item.EnumerateObject()) _ = res2.Append(JsonHandler(el.Value));
                    return res2;
                case JsonValueKind.String:
                    var s = JsonHandler(@item.GetString() ?? "");
                    // Log.Write("JS: '" + @item.GetString() + "' -> '" + s + "'");
                    return s;
                case JsonValueKind.Number:
                    if (@item.ToString().Contains('.')) return @item.GetDouble();
                    // Иначе это целое число
                    long  a = @item.GetInt64();
                    int   b = @item.GetInt32();
                    short c = @item.GetInt16();
                    if (a != b) return a;
                    if (b != c) return b;
                    return c;
                case JsonValueKind.True: return true;
                case JsonValueKind.False: return false;
                case JsonValueKind.Null: return null;
                }
            }
            Log.Write("JT: " + obj.GetType());

            return obj;
        }
        public static object? Json2obj(string json) {
            json = json.Trim();
            if (json.Length == 0) return null;

            object? data;
            if (json[0] == '[') data = JsonSerializer.Deserialize<List<object?>>(json);
            else if (json[0] == '{') data = JsonSerializer.Deserialize<Dictionary<string, object?>>(json);
            else return null;

            return JsonHandler(data);
        }



        private static Dictionary<string, object> GetXmlData(XElement xml) {
            var attr = xml.Attributes().ToDictionary(d => d.Name.LocalName, d => (object) d.Value);
            if (xml.HasElements) attr.Add("_value", xml.Elements().Select(GetXmlData));
            else if (!xml.IsEmpty) attr.Add("_value", xml.Value);

            return new Dictionary<string, object> { { xml.Name.LocalName, attr } };
        }

        public static string Json2xml(string json) => XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new XmlDictionaryReaderQuotas())).ToString();
        public static object Xml2obj(string xml) => GetXmlData(XElement.Parse(xml));
        public static string Xml2json(string xml) => Obj2json(GetXmlData(XElement.Parse(xml)));



        public static void RenderToFile(Control tar, string path) {
            var target = (Control?) tar.Parent;
            if (target == null) return;

            double w = target.Bounds.Width, h = target.Bounds.Height;
            var pixelSize = new PixelSize((int) w, (int) h);
            var size = new Size(w, h);
            using RenderTargetBitmap bitmap = new(pixelSize);
            target.Measure(size);
            target.Arrange(new Rect(size));
            bitmap.Render(target);
            bitmap.Save(path);
        }
    }
}
