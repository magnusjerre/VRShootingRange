using UnityEngine;
using System;

public class Waver2 : MonoBehaviour {

	[SerializeField] private Transform targetTransform;
	[SerializeField] private float[] animationTimes;
	[SerializeField] private float rotation = 45;
	[SerializeField] private Vector3 RotationAxis = Vector3.forward;
	[SerializeField] private bool initOffset = false;

	private int angleIndex, timeIndex;
	private float elapsedTime, speed, sumRotation;
	private Quaternion startRot, endRot, cStart, cEnd;

	void Awake() {
		if (animationTimes.Length == 0)
		{
			throw new IndexOutOfRangeException("There must be at least on time");
		}

		if (targetTransform == null)
		{
			targetTransform = transform;
		}
	}

	// Use this for initialization
	void Start () {
		if (initOffset) {
			transform.Rotate (RotationAxis * -rotation / 2);
		}

		startRot = transform.localRotation;
		transform.Rotate (RotationAxis * rotation);
		endRot = transform.localRotation;
		transform.rotation = startRot;
		cStart = startRot;
		cEnd = endRot;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= animationTimes [timeIndex] ) {
			elapsedTime = 0;
			timeIndex = (timeIndex + 1) % animationTimes.Length;
			SwapRotations ();
		}

		transform.localRotation = Quaternion.Lerp (cStart, cEnd, elapsedTime / animationTimes [timeIndex]);
	}

	private void SwapRotations() {
		Quaternion temp = cStart;
		cStart = cEnd;
		cEnd = temp;
	}
}
