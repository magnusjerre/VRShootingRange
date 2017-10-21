using UnityEngine;

public interface IHittable
{
    Hit RegisterHit(RaycastHit hit);
}