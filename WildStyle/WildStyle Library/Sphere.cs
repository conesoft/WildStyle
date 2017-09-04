using System;
using System.Numerics;

namespace WildStyle
{
    public struct Sphere
    {
        public Vector3 Origin;
        public float Radius;

        public float? Intersects(Ray ray)
        {
            var distance = ray.Origin - Origin;
            var B = Vector3.Dot(distance, ray.Direction);
            var C = Vector3.Dot(distance, distance) - Radius * Radius;
            var D = B * B - C;
            return D > 0 ? -B - (float)Math.Sqrt(D) : default(float?);
        }
    }
}
