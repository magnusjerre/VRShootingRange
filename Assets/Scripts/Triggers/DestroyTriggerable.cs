using UnityEngine;

namespace Jerre
{
    public class DestroyTriggerable : BaseTriggerable
    {
		[SerializeField] private bool FindTargetGameObject = true;

        void Start()
        {

        }

        public override void Trigger()
        {
            NotifyListeners();
			if (FindTargetGameObject) {
				Destroy (gameObject.GetComponentInParent<Target> ().gameObject);
			} else {
				Destroy (gameObject);
			}
        }

    }
}