using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicEmitter : MonoBehaviour {

	private Renderer renderer;
	[SerializeField] private Color emissionA, emissionB;
	[SerializeField] private float time;
	private float elapsedTime;
	private Color startColor, endColor;
	private bool animate = false;

	// Use this for initialization
	void Start () {
		startColor = emissionA;
		endColor = emissionB;
		renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!animate) {
			return;
		}

		elapsedTime += Time.deltaTime;
		if (elapsedTime >= time) {
			elapsedTime = 0f;
			var tempColor = endColor;
			endColor = startColor;
			startColor = tempColor;
		}
		var color = Color.Lerp(startColor, endColor, elapsedTime / time);
		renderer.material.SetColor("_EmissionColor", color);
	}

	public void StartEmission() {
		animate = true;
		elapsedTime = 0f;
		startColor = emissionA;
		endColor = emissionB;
	}

	public void StopEmission() {
		animate = false;
		renderer.material.SetColor("_EmissionColor", emissionA);
	}
}
