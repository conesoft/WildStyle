using System.Numerics;

namespace WildStyle
{
    public struct Glowing : IMaterial
    {
        public float Illumination;

        public (Vector3 diffuse, float illuminating) At(Vector3 world, Vector3 local) => (Vector3.Zero, Illumination);
    }
}
