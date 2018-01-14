using UnityEngine;

namespace Jerre
{
    public class ScalerTrigger : BaseTriggerable
    {

		public Vector3 scaleTo = Vector3.zero;
		public float time;

		private float elapsedTime;
		private Vector3 startScale;

		private bool animate;

        // Update is called once per frame
        void Update()
        {
			if (!animate) {
				return;
			}

			elapsedTime += Time.deltaTime;
			if (elapsedTime >= time) {
				animate = false;
				NotifyListeners();
			}

			transform.localScale = Vector3.Lerp(startScale, scaleTo, elapsedTime / time);
        }

		public override void Trigger()
        {
            if (animate)
            {
                Debug.Log("Already animating rotation");
                return;
            }

            animate = true;
            startScale = transform.localScale;
        }
    }
}