using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class AnimatorControllerModifier : MonoBehaviour {
	private Animator animator;
	public bool randomSpeed;
	public float minSpeed = 0.3f, maxSpeed = 0.7f, speed = 1;

	void Awake() {
		animator = GetComponent<Animator>();
		if (randomSpeed) {
			speed = Random.Range(minSpeed, maxSpeed);
		}
		animator.speed = speed;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.speed != speed) {
			animator.speed = speed;
		}
	}
}
