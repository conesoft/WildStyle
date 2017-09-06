using System.Numerics;

namespace WildStyle
{
    public interface IMaterial
    {
        (Vector3 diffuse, float illuminating) At(Vector3 world, Vector3 local);
    }
}
