using System;
using UnityEngine;

namespace Jerre {
	public class WaveScaler : MonoBehaviour {

		public Transform targetTransform;

		public Vector3[] DeltaScales;
		public float[] AnimationTimes;

		private Vector3 targetScale, previousScale, initScale;

		private int scaleEndIndex, currentTimeIndex;
		private float elapsedTime;

		void Awake () {
			if (DeltaScales.Length < 2) {
				throw new IndexOutOfRangeException ("Angles must have a length of at least 2");
			}

			if (AnimationTimes.Length == 0) {
				throw new IndexOutOfRangeException ("There must be at least on time");
			}

			if (targetTransform == null) {
				targetTransform = transform;
			}
		}

		void Start () {
			initScale = transform.localScale;
			previousScale = transform.localScale;
			Vector3 scale = DeltaScales [scaleEndIndex];
			targetScale = new Vector3(scale.x * initScale.x, scale.y * initScale.y, scale.z * initScale.z);
		}

		void Update () {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= AnimationTimes [currentTimeIndex]) {
				elapsedTime = 0;
				scaleEndIndex = (scaleEndIndex + 1) % DeltaScales.Length;
				currentTimeIndex = (currentTimeIndex + 1) % AnimationTimes.Length;
				previousScale = targetScale;
				Vector3 scale = DeltaScales [scaleEndIndex];
				targetScale = new Vector3(scale.x * initScale.x, scale.y * initScale.y, scale.z * initScale.z);
			}
			
			targetTransform.localScale = Vector3.Lerp (previousScale, targetScale, elapsedTime / AnimationTimes [currentTimeIndex]);
		}

		public void ApplyCustomisation (Vector3[] scales, float[] animationTimes) {
			if (scales != null && scales.Length > 0) {
				this.DeltaScales = scales;
			}
			if (animationTimes != null && animationTimes.Length > 0) {
				this.AnimationTimes = animationTimes;
			}
		}
	}
}
