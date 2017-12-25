using UnityEngine;
using System.Collections;


namespace Jerre {
	public class HideMeshTrigger : BaseTriggerable
	{

		// Use this for initialization
		void Start ()
		{
			
		}
		
		public override void Trigger() 
		{
			var renderers = GetComponentsInChildren<MeshRenderer> ();
			for (var i = 0; i < renderers.Length; i++) {
				var renderer = renderers[i];
				renderer.enabled = false;
				var collider = renderer.GetComponent<Collider> ();
				if (collider != null) {
					collider.enabled = false;
				}
			}
			NotifyListeners ();
		}
	}
}
	