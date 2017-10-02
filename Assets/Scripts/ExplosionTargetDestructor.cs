using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTargetDestructor : MonoBehaviour, IDestructor
{
    private ParticleSystem particleSystem;
    private Collider collider;

    [SerializeField]
    private List<MeshRenderer> meshRendererToDisable; 
    
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem == null)
        {
            throw new NullReferenceException("ExplostionTargetDestrcutor requires a child to have a ParticleSystem");
        }
        
        collider = GetComponent<Collider>();
        MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
        if (thisMeshRenderer != null && !meshRendererToDisable.Contains(thisMeshRenderer))
        {
            meshRendererToDisable.Add(thisMeshRenderer);            
        }
    }
    
    
    public void DestroyTarget()
    {
        DestroyAsSubTarget();
        particleSystem.Play();
    }

    public void DestroyAsSubTarget()
    {
        foreach (var meshRenderer in meshRendererToDisable)
        {
            meshRenderer.enabled = false;
        }
        collider.enabled = false;
    }
}