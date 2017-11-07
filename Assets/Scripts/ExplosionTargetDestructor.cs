using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class ExplosionTargetDestructor : MonoBehaviour, IDestructor
    {
        [SerializeField] private ParticleSystem particleSystem;
        private Collider collider;

        [SerializeField]
        private List<MeshRenderer> meshRendererToDisable;

        void Start()
        {
            SetParticleSystem();

            collider = GetComponent<Collider>();
            MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
            if (thisMeshRenderer != null && !meshRendererToDisable.Contains(thisMeshRenderer))
            {
                meshRendererToDisable.Add(thisMeshRenderer);
            }
        }

        private void SetParticleSystem()
        {
            if (particleSystem == null)
            {
                particleSystem = GetComponent<ParticleSystem>();
            }

            if (particleSystem == null)
            {
                particleSystem = GetComponentInChildren<ParticleSystem>();
            }

            if (particleSystem == null)
            {
                throw new NullReferenceException("ExplostionTargetDestrcutor requires a child to have a ParticleSystem");
            }
        }

        public void DestroyTarget()
        {
            DestroyAsSubTarget();
            particleSystem.Play();
            Invoke("DestroyThisObject", particleSystem.main.duration);
        }

        public void DestroyAsSubTarget()
        {
            foreach (var meshRenderer in meshRendererToDisable)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }

        private void DestroyThisObject()
        {
            Destroy(gameObject);
        }
    }
}