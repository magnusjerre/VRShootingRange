using UnityEngine;
using System.Collections;

namespace Jerre {
	public class ParticleTrigger : BaseTriggerable
	{

		[SerializeField] private ParticleSystem particleSystemPrefab;


		// Use this for initialization
		void Start ()
		{
			
		}

		public override void Trigger() {
			Debug.Log ("Trigger called");
			var ps = Instantiate (particleSystemPrefab, transform);
			var shape = ps.shape;
			Invoke ("NotifyListeners", ps.main.duration);
		}
		
	}
}
