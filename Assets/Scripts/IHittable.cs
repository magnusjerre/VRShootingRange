using UnityEngine;

namespace Jerre
{
    public interface IHittable
    {
        Hit RegisterHit(RaycastHit hit);
    }
}