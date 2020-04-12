using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WildStyle
{
    public class Tracer
    {
        readonly List<Sphere> spheres = new List<Sphere>();

        internal void Trace(int width, int height, int dx, int dy, Camera camera, Vector3[,] frame)
        {
            var r = new Random();
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
                    frame[x / dx, y / dy] += Trace(ray, 3, r) / (dx * dy);
                }
            }
        }

        internal void TraceParallel(int width, int height, int dx, int dy, Camera camera, Vector3[,] frame)
        {
            var scale = 1f / (dx * dy);
            Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = 128 }, y =>
            {
                var r = new Random(y);
                for (var x = 0; x < width; x++)
                {
                    frame[x, y] = default;
                    for (var dyi = 0; dyi < dy; dyi++)
                    {
                        for (var dxi = 0; dxi < dx; dxi++)
                        {
                            var ray = camera.CreateRay(x + ((float)dxi / dx), y + ((float)dyi / dy), width, height);
                            frame[x, y] += Trace(ray, 3, r) * scale;
                        }
                    }
                }
            });
        }

        public IMaterial CreateGlowingMaterial(float v) => new Glowing
        {
            Illumination = v
        };

        private (Sphere sphere, float distance)? TraceHit(Ray ray)
        {
            var hitSphere = default(Sphere);
            var distance = float.PositiveInfinity;

            for(var i = 0; i < spheres.Count; i++)
            {
                var s = spheres[i];

                var intersection = s.Intersects(ray);

                if(intersection.HasValue && intersection.Value < distance && intersection.Value > 0)
                {
                    hitSphere = s;
                    distance = intersection.Value;
                }
            }

            if(distance < float.PositiveInfinity)
            {
                return (hitSphere, distance);
            }
            return null;
            //return spheres
            //    .Select(s => (s, distance: s.Intersects(ray)))
            //    .Where(_ => _.distance.HasValue && _.distance.Value > 0f)
            //    .OrderBy(_ => _.distance)
            //    .Select(_ => (_.s, distance: _.distance.Value));
        }

        private Vector3 Trace(Ray ray, int iterations, Random r)
        {
            var possibleHit = TraceHit(ray);

            if (possibleHit != null)
            {
                var hit = possibleHit.Value;
                var point = hit.distance * ray.Direction + ray.Origin;
                var material = hit.sphere.Material.At(point, (point - hit.sphere.Origin));

                if (iterations <= 0)
                {
                    return new Vector3(material.illuminating);
                }

                var normal = (point - hit.sphere.Origin) / hit.sphere.Radius;

                var randomSample = new Ray
                {
                    Origin = point,
                    Direction = r.RandomVectorTowards(normal)
                };

                var shade = Trace(randomSample, iterations - 1, r) * Vector3.Dot(randomSample.Direction, normal);

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
