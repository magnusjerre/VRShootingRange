using System;
using JetBrains.Annotations;
using UnityEngine;

public class Waver : MonoBehaviour
{
	public Transform targetTransform;
	
	[SerializeField] private float[] DeltaAngles;
	[SerializeField] private float[] AnimationTimes;
	[SerializeField] private Vector3 RotationAxis = Vector3.right;
	[SerializeField] private Space space;

	private int angleEndIndex, currentTimeIndex;
	private float elapsedTime;

	private float targetRotation, totalRotation = 0f, speed;

	void Awake()
	{
		if (DeltaAngles.Length < 2)
		{
			throw new IndexOutOfRangeException("Angles must have a length of at least 2");
		}

		if (AnimationTimes.Length == 0)
		{
			throw new IndexOutOfRangeException("There must be at least on time");
		}

		if (targetTransform == null)
		{
			targetTransform = transform;
		}
	}

	void Start()
	{
		targetRotation = DeltaAngles[0];
		speed = targetRotation / AnimationTimes[0];
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= AnimationTimes[currentTimeIndex])
		{
			elapsedTime = 0;
			angleEndIndex = (angleEndIndex + 1) % DeltaAngles.Length;
			currentTimeIndex = (currentTimeIndex + 1) % AnimationTimes.Length;
			targetRotation = DeltaAngles[angleEndIndex];
			speed = targetRotation / AnimationTimes[currentTimeIndex];
		}

		var deltaRot = speed * Time.deltaTime;

		var temp = deltaRot + totalRotation;
		if (deltaRot < 0 && temp < targetRotation)
		{
			deltaRot = Mathf.Max(deltaRot, totalRotation - targetRotation);
		}
		else if (deltaRot > 0 && temp > targetRotation)
		{
			deltaRot = Mathf.Min(targetRotation - totalRotation);
		}
		totalRotation += deltaRot;
		targetTransform.Rotate(deltaRot * RotationAxis, space);
	}
}
