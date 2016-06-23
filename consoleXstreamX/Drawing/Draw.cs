using System.Drawing;

namespace consoleXstreamX.Drawing
{
    internal static class Draw
    {
        private const string FontName = "Tahoma";

        private static Bitmap _display;
        private static Graphics _graph;
        private static readonly Brush FontBrush = Brushes.Black;

        public enum HorizontalAlignment
        {
            Left, 
            Middle,
            Right
        }

        public enum VerticalAlignment
        {
            Top,
            Middle,
            Bottom
        }

        public static VerticalAlignment SetVertical;
        public static HorizontalAlignment SetHorizontal;

        public static bool Outline { get; set; }
        public static float FontSize { get; set; }

        static Draw()
        {
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
                OutlineText(x, y, value);
                return;
            }

            _graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            var textRect = new RectangleF(x, y, x + textSize.Width, y + textSize.Height);

            if (SetHorizontal == HorizontalAlignment.Middle)
            {
                //var offset = ((rect.Width - rect.X) - textRect.Width) / 2;
                //textRect.X += offset;
            }
            _graph.DrawString(value, font, FontBrush, textRect);
        }

        private static void OutlineText(int intX, int intY, string value)
        {
            /*
            if (_listOutline.Count > 0)
            {
                for (int intCount = 0; intCount < _listOutline.Count; intCount++)
                {
                    bool boolFound = true;
                    if (_listOutline[intCount].strWrite != strWrite) boolFound = false;
                    if (_listOutline[intCount].strFontName != FontName) boolFound = false;
                    if (_listOutline[intCount].floatSize != _floatFontSize) boolFound = false;

                    if (boolFound)
                    {
                        DrawImage(intX, intY, _listOutline[intCount].bmpImage);
                        return;
                    }
                }
            }

            Font fontSet = new Font(FontName, _floatFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF textSize = _graph.MeasureString(strWrite, fontSet);

            Bitmap bmpFontoutline = new Bitmap((int)textSize.Width + 20, (int)textSize.Height + 20);
            Graphics graph = Graphics.FromImage(bmpFontoutline);

            StringFormat strFormat = new StringFormat();
            strFormat.LineAlignment = StringAlignment.Near;

            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            Pen penOutline = new Pen(_colOutline, OutlineSize);
            penOutline.LineJoin = LineJoin.Round;

            Rectangle rectText = new Rectangle(intX, intY, (int)(intX + textSize.Width), (int)(intY + textSize.Height));
            LinearGradientBrush gradBrush = new LinearGradientBrush(rectText, _colorFont, _colorFontMinor, 90);

            RectangleF textRect = new RectangleF(0, 0, textSize.Width, textSize.Height);

            Brush fontBrush = Brushes.Black;

            GraphicsPath graphPath = new GraphicsPath();

            graphPath.AddString(strWrite, fontSet.FontFamily, (int)fontSet.Style, _floatFontSize, textRect, strFormat);

            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graph.DrawPath(penOutline, graphPath);
            graph.FillPath(gradBrush, graphPath);

            //Clean up
            graphPath.Dispose();
            gradBrush.Dispose();
            fontSet.Dispose();
            strFormat.Dispose();

            DrawImage(intX, intY, bmpFontoutline);

            //Store the image
            _listOutline.Add(new OutlineText());

            int intIndex = _listOutline.Count - 1;
            _listOutline[intIndex].strWrite = strWrite;
            _listOutline[intIndex].strFontName = FontName;
            _listOutline[intIndex].floatSize = _floatFontSize;
            _listOutline[intIndex].bmpImage = bmpFontoutline;
            */
        }

        public static Bitmap GetImage()
        {
            _graph.Flush();
            _graph.Dispose();

            return _display;
        }

    }
}
