﻿using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TargetCollider : MonoBehaviour
{
    [SerializeField] private GameObject ownerForCollider;

    public void SetOwner(GameObject owner)
    {
        ownerForCollider = owner;
    }

    public T GetOwner<T>()
    {
        return ownerForCollider.GetComponent<T>();
    }

    void Start()
    {
        if (ownerForCollider == null)
        {
            throw new NullReferenceException("TargetCollider must have a Target owner");
        }
    }
    
}