using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TargetCollider : MonoBehaviour
{
    [SerializeField] private Target ownerForCollider;

    public Target GetTarget()
    {
        return ownerForCollider;
    }

    void Start()
    {
        if (ownerForCollider == null)
        {
            throw new NullReferenceException("TargetCollider must have a Target owner");
        }
    }
    
}