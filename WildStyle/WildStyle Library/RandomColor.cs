using System.Numerics;

namespace WildStyle
{
    public struct RandomColor : IMaterial
    {
        static readonly Random r = new Random();

        public (Vector3 diffuse, float illuminating) At(Vector3 world, Vector3 local)
        {
            return (new Vector3(r.NextUnsignedFloat(), r.NextUnsignedFloat(), r.NextUnsignedFloat()), 0f);
        }
    }
}
