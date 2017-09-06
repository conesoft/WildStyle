using System.Numerics;

namespace WildStyle
{
    public class Camera
    {
        private Tracer tracer;

        internal Camera(Tracer tracer)
        {
            this.tracer = tracer;
        }

        internal Ray CreateRay(float x, float y, int width, int height)
        {
            var aspect = (float)width / (float)height;

            var dx = (2f * x - width) / (2f * width);
            var dy = (2f * y - height) / (2f * height * aspect);

            return new Ray
            {
                Origin = Vector3.Zero,
                Direction = Vector3.Normalize(Vector3.UnitZ + dx * Vector3.UnitX + dy * Vector3.UnitY)
            };
        }

        public Vector3[,] Snapshot(int width, int height, int dx, int dy)
        {
            var frame = new Vector3[width, height];
            tracer.Trace(width, height, dx, dy, this, frame);
            return frame;
        }
    }
}
