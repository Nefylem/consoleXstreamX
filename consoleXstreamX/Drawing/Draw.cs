using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using consoleXstreamX.Configuration;
using consoleXstreamX.Properties;

namespace consoleXstreamX.Drawing
{
    internal static class Draw
    {
        private const string FontName = "Tahoma";

        private static Bitmap _display;
        private static Graphics _graph;
        private static readonly Color MajorFontColor = Color.White;
        private static readonly Color MinorFontColor = Color.WhiteSmoke;
        private static readonly Brush FontBrush = Brushes.Black;
        private static readonly Color OutlineColor = Color.Black;
        private const int OutlineSize = 4;

        public enum HorizontalAlignment
        {
            Left, 
            Middle,
            Right
        }

        public static HorizontalAlignment SetHorizontal;

        public enum VerticalAlignment
        {
            Top,
            Middle,
            Bottom
        }

        public static VerticalAlignment SetVertical;

        public static bool Outline { get; set; }
        public static float FontSize { get; set; }

        private class OutlineCache
        {
            public Bitmap Tile;
            public string Display;
            public string Font;
            public float Size;
        }

        private static readonly List<OutlineCache> _outlineCache;

        static Draw()
        {
            _outlineCache = new List<OutlineCache>();
            FontSize = 12;
        }

        public static void ClearGraph(Bitmap bitmap)
        {
            _display = bitmap;
            _graph = Graphics.FromImage(bitmap);
        }

        public static void Image(int x, int y, Bitmap image)
        {
            _graph.DrawImage(image, new Point(x, y));
        }

        public static void Image(int x, int y, int w, int h, Bitmap image)
        {
            //Convert width / height to ratio of original
            double width = image.Width;
            width /= 100;
            width *= w;

            double height = image.Height;
            height /= 100;
            height *= h;

            _graph.DrawImage(image, new Rectangle(x, y, (int)width, (int)height));
        }

        public static void Text(Rectangle rect, object write)
        {
            var value = write?.ToString();
            if (string.IsNullOrEmpty(value)) return;
            value = value.Trim();

            var font = new Font(FontName, FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            var textSize = _graph.MeasureString(value, font);

            //Default to top left
            var x = rect.X;
            var y = rect.Y;

            if (SetVertical == VerticalAlignment.Top)
                y = (int)(rect.Top - textSize.Height);

            if (SetVertical == VerticalAlignment.Bottom)
                y = (int)(rect.Bottom - textSize.Height) - 5;

            if (Outline)
            {
                OutlineText(rect.X, rect.Y, value);
                return;
            }

            _graph.SmoothingMode = SmoothingMode.AntiAlias;
            _graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            _graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var textRect = new RectangleF(x, y, x + textSize.Width, y + textSize.Height);

            if (SetHorizontal == HorizontalAlignment.Middle)
            {
                var rectWidth = textRect.Width - textRect.X;
                var tileWidth = rect.Width;
                textRect.X += ((tileWidth - rectWidth) / 2);
            }
            _graph.DrawString(value, font, FontBrush, textRect);
        }

        private static void OutlineText(int x, int y, object write)
        {
            var value = write.ToString();
            var cachedImage = _outlineCache.FirstOrDefault(s => s.Display == value && s.Font == FontName && s.Size == FontSize);
            if (cachedImage != null)
            {
                if (SetVertical == VerticalAlignment.Top) y += 5;
                if (SetVertical == VerticalAlignment.Bottom)
                {
                    var height = cachedImage.Tile.Height;
                    if (height == 59) height = 50;
                    y += Resources.tile_low.Height - height;
                }
                if (SetHorizontal == HorizontalAlignment.Left) x += 5;
                if (SetHorizontal == HorizontalAlignment.Middle)
                {
                    //Image size doesnt return correctly. So find the font size and go from there
                    var font = new Font(FontName, FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    var checkSize = _graph.MeasureString(value, font);
                    var checkText = new RectangleF(x, y, x + checkSize.Width, y + checkSize.Height);
                    var rectWidth = checkText.Width - checkText.X;
                    var tileWidth = MenuSettings.CellWidth;
                    x += (int)((tileWidth - rectWidth) / 2);
                }
                Image(x, y, cachedImage.Tile);
                return;
            }

            var fontSet = new Font(FontName, FontSize + 5, FontStyle.Bold, GraphicsUnit.Pixel);
            var textSize = _graph.MeasureString(value, fontSet);

            var image = new Bitmap((int)textSize.Width + 1, (int)textSize.Height + 1);
            var graph = Graphics.FromImage(image);
            //SolidBrush blueBrush = new SolidBrush(Color.Red);
            //graph.FillRectangle(blueBrush, 0, 0, image.Width, image.Height);

            var stringFormat = new StringFormat { LineAlignment = StringAlignment.Near };

            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var penOutline = new Pen(OutlineColor, OutlineSize) {LineJoin = LineJoin.Round};

            var textRectangle = new Rectangle(x, y, (int)(x + textSize.Width), (int)(y + textSize.Height));
            var gradBrush = new LinearGradientBrush(textRectangle, MajorFontColor, MinorFontColor, 90);

            var textRect = new RectangleF(0, 0, textSize.Width, textSize.Height);
            var graphPath = new GraphicsPath();

            graphPath.AddString(value, fontSet.FontFamily, (int)fontSet.Style, FontSize + 5, textRect, stringFormat);

            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graph.DrawPath(penOutline, graphPath);
            graph.FillPath(gradBrush, graphPath);

            //Clean up
            graphPath.Dispose();
            gradBrush.Dispose();
            fontSet.Dispose();
            stringFormat.Dispose();

            Image(x, y, image);

            _outlineCache.Add(new OutlineCache()
            {
                Tile = image,
                Display = value,
                Font = FontName,
                Size = FontSize
            });
        }

        public static Bitmap GetImage()
        {
            _graph.Flush();
            _graph.Dispose();

            return _display;
        }

    }
}
