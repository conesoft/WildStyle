using System.Numerics;

namespace WildStyle
{
    public struct SolidColor : IMaterial
    {
        public Vector3 Color;

        public (Vector3 diffuse, float illuminating) At(Vector3 world, Vector3 local) => (Color, 0f);
    }
}
