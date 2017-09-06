using System;
using System.Numerics;

namespace WildStyle
{
    public struct RandomColor : IMaterial
    {
        static Random r = new Random();

        public (Vector3 diffuse, float illuminating) At(Vector3 world, Vector3 local)
        {
            return (new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), 0f);
        }
    }
}
