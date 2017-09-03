using System;
using System.Threading.Tasks;
using WildStyle;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Simple_Demo_Application
{
    public sealed partial class MainPage : Page
    {
        Tracer tracer;

        public MainPage()
        {
            this.InitializeComponent();

            tracer = new Tracer();

            BeginRender();
        }

        private void BeginRender()
        {
            var camera = tracer.CreateCamera();
            var image = camera.Snapshot(320, 180);

            var bitmap = new WriteableBitmap(320, 180);
            bitmap.ForEach((x, y) =>
            {
                var sample = image[x, y] * 255;
                return Color.FromArgb(255, (byte)sample.X, (byte)sample.Y, (byte)sample.Z);
            });
            screen.Source = bitmap;
        }
    }
}
