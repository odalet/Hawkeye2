using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HawkeyeLogo
{
    internal static class Utils
    {
        public static void SaveTo<T>(this T control, int width, int height, string filename) where T : Control, new()
        {
            var savedBackground = control.Background;
            var savedMargin = control.Margin;

            control.Margin = new Thickness(0.0);
            control.Background = new SolidColorBrush(Colors.Transparent);

            var size = new Size(width, height);

            control.Measure(size);
            control.Arrange(new Rect(size));

            var bitmap = new RenderTargetBitmap(
               width, height, 96.0, 96.0, PixelFormats.Pbgra32);
            bitmap.Render(control);

            var image = new PngBitmapEncoder();
            image.Frames.Add(BitmapFrame.Create(bitmap));
            using (var fs = File.Create(filename))
                image.Save(fs);

            control.Margin = savedMargin;
            control.Background = savedBackground;
        }
    }
}
