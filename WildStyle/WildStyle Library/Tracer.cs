using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WildStyle
{
    public class Tracer
    {
        List<Sphere> spheres = new List<Sphere>();

        internal void Trace(int width, int height, Camera camera, Vector3[,] frame)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var ray = camera.CreateRay(x, y, width, height);
                    frame[x, y] = Trace(ray);
                }
            }
        }

        private Vector3 Trace(Ray ray)
        {
            var spheresHit = spheres
                .Select(s => (s, distance: s.Intersects(ray)))
                .Where(_ => _.distance.HasValue)
                .OrderBy(_ => _.distance);

            if (spheresHit.Any())
            {
                var sphere = spheresHit.First();
                return Vector3.UnitX;
            }
            return Vector3.Zero;
        }

        public Camera CreateCamera() => new Camera(this);

        public void AddSphere(Vector3 origin, float radius) => spheres.Add(new Sphere
        {
            Origin = origin,
            Radius = radius
        });
    }
}
