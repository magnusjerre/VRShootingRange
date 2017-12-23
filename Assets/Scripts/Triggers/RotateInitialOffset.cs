using UnityEngine;

namespace Jerre
{
    public class RotateInitialOffset : BaseTriggerable
    {
        public float rotationAmount;
        public Vector3 rotationAxis;
		public bool rotateOnAwake;

		void Awake() 
		{
			base.Awake ();
			if (rotateOnAwake) {
				transform.Rotate (rotationAxis * rotationAmount);
			}
		}

        void Start()
        {
        }

        public override void Trigger()
        {
            transform.Rotate(rotationAxis * rotationAmount);
            NotifyListeners();
        }

    }
}