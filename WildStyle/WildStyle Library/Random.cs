using System.Numerics;

namespace WildStyle
{
    public class Random : System.Random
    {
        public Random() : base()
        {
        }

        public Random(int seed) : base(seed)
        {
        }

        internal float NextUnsignedFloat() => (float)NextDouble();
        internal float NextSignedFloat() => (float)NextDouble() * 2f - 1f;


        internal Vector3 RandomVectorTowards(Vector3 normal)
        {
            var v = Vector3.Zero;
            var l = 0f;
            do
            {
                v.X = NextSignedFloat();
                v.Y = NextSignedFloat();
                v.Z = NextSignedFloat();
                l = Vector3.Dot(v, v);
            } while (l > 1 && l < 0.1f);
            return Vector3.Normalize(Vector3.Dot(v, normal) > 0 ? v : -v);
        }
    }
}
