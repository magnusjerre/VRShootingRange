using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class TranslatorTrigger : BaseTriggerable
    {
		public Vector3 translate;

		public float time;
		public Space space;
		private float elapsedTime;
		private Vector3 startPos;
		private bool animate;


        // Use this for initialization
        void Start()
        {

        }

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


			Vector3 position = Vector3.Lerp(startPos, translate, elapsedTime / time);
			if (space == Space.World) {
				transform.position = position;
			} else {
				transform.localPosition = position;
			}
        }

		public override void Trigger()
        {
            if (animate)
            {
                Debug.Log("Already animating rotation");
                return;
            }

            animate = true;
            startPos = space == Space.World ? transform.position : transform.localPosition;
        }

    }
}
