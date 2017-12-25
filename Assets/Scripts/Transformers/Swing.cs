using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour
{

	[SerializeField] private float maxAngle, damping = 0.95f, startRotationTime;
	[SerializeField] private Vector3 rotationAxis;

	private float currentMaxAngle, sumAngle, currentRotationTime, direction, currentVelocity;

	private float PI2 = Mathf.PI * 2f, PIHalf = Mathf.PI / 2f;
	private float elapsedTime;

	// Use this for initialization
	void Start ()
	{
		transform.Rotate (rotationAxis * -maxAngle);
		sumAngle = 0;
		currentMaxAngle = maxAngle * 2 * Mathf.Deg2Rad;
		currentRotationTime = startRotationTime;
		direction = 1;
		currentVelocity = currentMaxAngle / currentRotationTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		elapsedTime += Time.deltaTime;
		var multiplier = GetSpeedMultiplier (elapsedTime / currentRotationTime);
		//Multiply by two because the top speed using sines will at most be the average speed. To counter act this, multiply by 2!
		var deltaRotation = multiplier * currentVelocity * 2 * Time.deltaTime;
		if (elapsedTime >= currentRotationTime) {
			direction *= -1;
			currentMaxAngle *= damping;
			currentRotationTime *= damping;
			currentVelocity *= damping;
			sumAngle = 0f;
			elapsedTime = 0f;
		}
		sumAngle += deltaRotation;
		transform.Rotate (rotationAxis * deltaRotation * Mathf.Rad2Deg * direction);
	}

	private float GetSpeedMultiplier(float amount) {
		var sin = amount * PI2 - PIHalf;
		var numerator = Mathf.Sin (sin) + 1;
		return numerator / 2f;
	}

	private float convertToSin(float elapsedTime, float totalTime) {
		return elapsedTime / totalTime;
	}
}

