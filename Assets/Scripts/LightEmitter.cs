using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightEmitter : MonoBehaviour {

	private Light light;
	public float minIntensity = 0.5f, maxIntensity = 1.5f; 
	public float intensityMeanTime = 1f;
	private float elapsedTime, currentMaxTime, nextIntensity, prevIntensity;	
	

	void Awake() {
		light = GetComponent<Light>();
	}

	// Use this for initialization
	void Start () {
		light.intensity = minIntensity;
		elapsedTime = 0;
		currentMaxTime = intensityMeanTime;
	}
	
	// Update is called once per frame
	void Update () {
		float intensity = 0f;
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= currentMaxTime) {
			elapsedTime = 0;
			currentMaxTime = intensityMeanTime + Random.Range(-0.3f, 0.3f);
			if (nextIntensity == maxIntensity) {
				nextIntensity = minIntensity;
				prevIntensity = maxIntensity;
			} else {
				nextIntensity = maxIntensity;
				prevIntensity = minIntensity;
			}
		}

		intensity = Mathf.Lerp(prevIntensity, nextIntensity, elapsedTime / currentMaxTime);
		light.intensity = intensity;
	}
}
