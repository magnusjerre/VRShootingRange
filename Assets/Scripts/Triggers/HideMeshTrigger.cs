using UnityEngine;
using System.Collections;


namespace Jerre {
	public class HideMeshTrigger : BaseTriggerable
	{

		public MeshRenderer[] renderersToHide;

		// Use this for initialization
		void Start ()
		{
			
		}
		
		public override void Trigger() 
		{
			for (var i = 0; i < renderersToHide.Length; i++) {
				renderersToHide [i].enabled = false;
				var collider = renderersToHide [i].GetComponent<Collider> ();
				if (collider != null) {
					collider.enabled = false;
				}
			}
			NotifyListeners ();
		}
	}
}
	