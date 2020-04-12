using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WildStyle
{
    public class Tracer
    {
        List<Sphere> spheres = new List<Sphere>();

        internal void Trace(int width, int height, int dx, int dy, Camera camera, Vector3[,] frame)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    frame[x, y] = default;
                }
            }
            for (var y = 0; y < height * dy; y++)
            {
                for (var x = 0; x < width * dx; x++)
                {
                    var ray = camera.CreateRay((float)x / dx, (float)y / dy, width, height);
                    frame[x / dx, y / dy] += Trace(ray, 3) / (dx * dy);
                }
            }
        }

        Random r = new Random();

        private Vector3 RandomVectorTowards(Vector3 normal)
        {
            var v = Vector3.Zero;
            var l = 0f;
            do
            {
                v.X = (float)r.NextDouble() * 2f - 1f;
                v.Y = (float)r.NextDouble() * 2f - 1f;
                v.Z = (float)r.NextDouble() * 2f - 1f;
                l = Vector3.Dot(v, v);
            } while (l > 1 && l < 0.1f);
            return Vector3.Normalize(Vector3.Dot(v, normal) > 0 ? v : -v);
        }

        public IMaterial CreateGlowingMaterial(float v) => new Glowing
        {
            Illumination = v
        };

        private IEnumerable<(Sphere s, float distance)> TraceHits(Ray ray)
        {
            return spheres
                .Select(s => (s, distance: s.Intersects(ray)))
                .Where(_ => _.distance.HasValue && _.distance.Value > 0f)
                .OrderBy(_ => _.distance)
                .Select(_ => (_.s, distance: _.distance.Value));
        }

        private Vector3 Trace(Ray ray, int iterations)
        {
            var spheresHit = TraceHits(ray);

            if (spheresHit.Any())
            {
                var sphere = spheresHit.First();
                var point = sphere.distance * ray.Direction + ray.Origin;
                var material = sphere.s.Material.At(point, (point - sphere.s.Origin));

                if (iterations <= 0)
                {
                    return new Vector3(material.illuminating);
                }

                var normal = (point - sphere.s.Origin) / sphere.s.Radius;

                var randomSample = new Ray
                {
                    Origin = point,
                    Direction = RandomVectorTowards(normal)
                };

                var shade = Trace(randomSample, iterations - 1) * Vector3.Dot(randomSample.Direction, normal);

                return new Vector3(material.illuminating) + shade * material.diffuse;
            }
            return new Vector3(-ray.Direction.Y * .5f + .5f) / 64f;
        }

        public Camera CreateCamera() => new Camera(this);

        public void AddSphere(Vector3 origin, float radius, IMaterial material) => spheres.Add(new Sphere
        {
            Origin = origin,
            Radius = radius,
            Material = material
        });

        public IMaterial CreateMaterial(float r, float g, float b) => new SolidColor
        {
            Color = new Vector3(r, g, b)
        };

        public IMaterial CreateRandomMaterial() => new RandomColor();
    }
}
