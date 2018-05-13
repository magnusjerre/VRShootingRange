using UnityEngine;

public class TranslatePingPong : MonoBehaviour {

	[SerializeField] Vector3 deltaDistance;
	[SerializeField] float[] times;

	Vector3 sumDelta, targetDelta, speed;
	float elapsedTime;
	int timeIndex;
	// Use this for initialization
	void Start () {
		targetDelta = deltaDistance;
		speed = deltaDistance / times [timeIndex];
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= times[timeIndex]) {
			transform.localPosition += Rest ();
			elapsedTime = 0f;
			targetDelta = targetDelta * -1;
			timeIndex = (timeIndex + 1) % times.Length;
			speed = targetDelta / times [timeIndex];
			sumDelta = Vector3.zero;
		}
		Vector3 delta = speed * Time.deltaTime;
		sumDelta += delta;
		transform.localPosition += delta;
	}

	Vector3 Rest() {
		float deltaX = Mathf.Abs (targetDelta.x - sumDelta.x);
		float x = targetDelta.x > 0 ? deltaX : -deltaX;

		float deltaY = Mathf.Abs (targetDelta.y - sumDelta.y);
		float y = targetDelta.y > 0 ? deltaY : -deltaY;

		float deltaZ = Mathf.Abs (targetDelta.z - sumDelta.z);
		float z = targetDelta.z > 0 ? deltaZ : -deltaZ;

		return new Vector3 (x, y, z);
	}

}
