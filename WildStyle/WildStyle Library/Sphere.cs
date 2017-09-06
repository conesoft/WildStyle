using System;
using System.Collections.Generic;
using System.Numerics;

namespace WildStyle
{
    public struct Sphere : IEquatable<Sphere>
    {
        public Vector3 Origin;
        public float Radius;

        public IMaterial Material;

        #region Equals & Hash

        public override bool Equals(object obj)
        {
            return obj is Sphere && Equals((Sphere)obj);
        }

        public bool Equals(Sphere other)
        {
            return Origin.Equals(other.Origin) &&
                   Radius == other.Radius &&
                   EqualityComparer<IMaterial>.Default.Equals(Material, other.Material);
        }

        public override int GetHashCode()
        {
            var hashCode = -1183347670;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Origin);
            hashCode = hashCode * -1521134295 + Radius.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IMaterial>.Default.GetHashCode(Material);
            return hashCode;
        }

        public static bool operator ==(Sphere sphere1, Sphere sphere2)
        {
            return sphere1.Equals(sphere2);
        }

        public static bool operator !=(Sphere sphere1, Sphere sphere2)
        {
            return !(sphere1 == sphere2);
        }

        #endregion

        public float? Intersects(Ray ray)
        {
            var distance = ray.Origin - Origin;
            var B = Vector3.Dot(distance, ray.Direction);
            var C = Vector3.Dot(distance, distance) - Radius * Radius;
            var D = B * B - C;
            return D > 0 ? -B - (float)Math.Sqrt(D) : default;
        }
    }
}
