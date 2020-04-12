using System;
using System.ComponentModel;
using System.Numerics;
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
        Random r = new Random();

        public MainPage()
        {
            this.InitializeComponent();

            tracer = new Tracer();

            var _ = BeginRender();
        }

        private async Task BeginRender()
        {
            var size = (width: 1280, height: 720);
            var image = default(Vector3[,]);
            await Task.Run(() =>
            {
                var orange = tracer.CreateMaterial(1f, .5f, .25f);
                var grey = tracer.CreateMaterial(.4f, .4f, .4f);
                var blue = tracer.CreateMaterial(.3f, .3f, .7f);
                var glow = tracer.CreateGlowingMaterial(12f);
                var slightlyGlowing = tracer.CreateGlowingMaterial(1f);
                tracer.AddSphere(Vector3.UnitZ * 4 - Vector3.UnitX * .5f, 1f, orange);
                tracer.AddSphere(Vector3.UnitZ * 3 + Vector3.UnitX * .35f - Vector3.UnitY * .25f, .25f, grey);
                tracer.AddSphere(Vector3.UnitZ * 3.2f - Vector3.UnitX * .45f - Vector3.UnitY * .45f, .25f, glow);
                //tracer.AddSphere((Vector3.UnitZ * 4 + Vector3.UnitY * 3.5f - Vector3.UnitX * .4f) * 6, 27f, blue);
                tracer.AddSphere(Vector3.UnitZ * 2 + Vector3.UnitX * 4 - Vector3.UnitY * 3.5f, 3f, slightlyGlowing);
                //tracer.AddSphere(Vector3.UnitZ * 3 + Vector3.UnitX * .45f - Vector3.UnitY * .35f, .25f, glow);

                var camera = tracer.CreateCamera();
                image = camera.Snapshot(size.width, size.height, 5, 5);
            });

            var bitmap = new WriteableBitmap(size.width, size.height);
            bitmap.ForEach((x, y) =>
            {
                var sample = image[x, y] * 44.5f;
                sample.X = 1 - (float)Math.Exp(-sample.X);
                sample.Y = 1 - (float)Math.Exp(-sample.Y);
                sample.Z = 1 - (float)Math.Exp(-sample.Z);
                sample *= 255;
                sample.Y = Math.Max(0, Math.Min(255, sample.Y + ((float)r.NextDouble() * 2f - 1f)));
                sample.Z = Math.Max(0, Math.Min(255, sample.Z + ((float)r.NextDouble() * 2f - 1f)));
                sample.X = Math.Max(0, Math.Min(255, sample.X + ((float)r.NextDouble() * 2f - 1f)));
                return Color.FromArgb(255, (byte)sample.X, (byte)sample.Y, (byte)sample.Z);
            });
            screen.Source = bitmap;
        }
    }
}
