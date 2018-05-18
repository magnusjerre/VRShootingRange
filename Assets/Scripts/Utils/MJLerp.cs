using UnityEngine;

namespace Jerre {
	public class MJLerp {
		public static float LerpSin(float amount) {
			return Mathf.Sin(amount * Mathf.PI * 0.5f);
		}

		public static float LerpBezier(float t) {
			Vector2 p0 = Vector2.zero;
			Vector2 p1 = new Vector2 (0.1f, 0.9f);
			Vector2 p2 = new Vector2 (0.4f, 0.95f);
			Vector2 p3 = Vector2.one;
			float p = 1 - t;
			return ((p * p * p * p0) + (3 * p * p * t * p1) + (3 * p * t * t * p2) + (t * t * t * p3)).y;
		}

		public static float Lerp(float amount, MJLerpMode mode) {
			switch(mode) {
			case MJLerpMode.BEZIER: {
					return LerpBezier (amount);
				}
			case MJLerpMode.BEZIER_INVERT: {
					return LerpBezier (1f - amount);
				}
			default: {
					return LerpSin (amount);
				}
			}
		}

	}
}
