using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WildStyle
{
    public class Tracer
    {
        List<(Vector3 center, float radius)> spheres = new List<(Vector3 center, float radius)>();

        internal void Trace(int width, int height, Camera camera, Vector3[,] frame)
        {
            for (var y = 0; y < height; y++)
            {
                for(var x = 0; x < width; x++)
                {
                    var ray = camera.CreateRay(x, y, width, height);
                    frame[x, y] = Trace(ray);
                }
            }
        }

        private Vector3 Trace((Vector3 origin, Vector3 direction) ray)
        {
            return IntersectsSphere(ray, spheres.First()).HasValue
                ? Vector3.UnitX
                : Vector3.Zero
                ;
        }

        public Camera CreateCamera() => new Camera(this);

        public void AddSphere(Vector3 center, float radius) => spheres.Add((center, radius));

        float? IntersectsSphere((Vector3 origin, Vector3 direction) ray, (Vector3 center, float radius) sphere)
        {
            var distance = ray.origin - sphere.center;
            var B = Vector3.Dot(distance, ray.direction);
            var C = Vector3.Dot(distance, distance) - sphere.radius * sphere.radius;
            var D = B * B - C;
            return D > 0 ? -B - (float)Math.Sqrt(D) : default(float?);
        }
    }

    public class Camera
    {
        private Tracer tracer;

        internal Camera(Tracer tracer)
        {
            this.tracer = tracer;
        }

        internal (Vector3 origin, Vector3 direction) CreateRay(int x, int y, int width, int height)
        {
            var aspect = (float)width / (float)height;

            var dx = (2f * x - width) / (2f * width);
            var dy = (2f * y - height) / (2f * height * aspect);

            return (
                origin: Vector3.Zero,
                direction: Vector3.Normalize(Vector3.UnitZ + dx * Vector3.UnitX + dy * Vector3.UnitY)
            );
        }

        public Vector3[,] Snapshot(int width, int height)
        {
            var frame = new Vector3[width, height];
            tracer.Trace(width, height, this, frame);
            return frame;
        }
    }
}
