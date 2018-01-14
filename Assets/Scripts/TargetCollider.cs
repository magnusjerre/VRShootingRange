using System;
using UnityEngine;

namespace Jerre
{
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
			if (ownerForCollider == null) {
				var target = GetComponentInParent<Target> ();
				if (target != null) {
					ownerForCollider = target.gameObject;
				}
			}

            if (ownerForCollider == null)
            {
                // throw new NullReferenceException("TargetCollider must have a Target owner");
            }
        }

    }
}