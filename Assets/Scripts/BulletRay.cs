using UnityEngine;
using System.Collections;
using Jerre;

namespace Jerre {
	public class BulletRay : MonoBehaviour, IHitlistener {

		public float MaxShotLength = 40f;

		[SerializeField] ParticleSystem particleSystemPrefab;

		ParticleSystem particleSystemInstance;

		void Awake() {
			particleSystemInstance = (ParticleSystem)Instantiate (particleSystemPrefab, transform);
		}

		void Start () {
		}

		public void Fire(Transform origin, IWeapon weapon, Transform calculationTransform, IHitlistener hitListener) {
			if (calculationTransform == null) {
				calculationTransform = origin;
			}
			Ray ray = new Ray(calculationTransform.position, calculationTransform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, MaxShotLength))
			{
				ShowShot (origin.position, hit.point);
				var targetCollider = hit.collider.GetComponent<TargetCollider>();
				if (targetCollider != null)
				{
					Hit tHit = targetCollider.GetOwner<IHittable>().RegisterHit(hit);
					if (hitListener != null)
					{
						hitListener.NotifyHit(tHit, weapon);
					}
				} 
				else if (hitListener != null)
				{
					hitListener.NotifyHit(Hit.Miss(), weapon);
				}
			}
			else
			{
				if (hitListener != null)
				{
					hitListener.NotifyHit(Hit.Miss(), weapon);
				}
				ShowShot (origin.position, origin.position + origin.forward * MaxShotLength);
			}
		}

		void ShowShot(Vector3 start, Vector3 end)
		{
			gameObject.SetActive(true);
			var shape = particleSystemInstance.shape;
			var length = (end - start).magnitude;
			var localPos = Vector3.forward * length / 2f;
			transform.position = start;
			transform.LookAt (end);   
			shape.radius = length / 2f;         
			shape.position = localPos;
			shape.rotation = Vector3.up * 90f;
			particleSystemInstance.Play();

			Invoke("Hide", particleSystemInstance.main.duration * 2f);
		}

		public void Hide()
		{
			particleSystemInstance.Stop ();
			gameObject.SetActive(false);
		}

		#region IHitlistener implementation

		public void NotifyHit (Hit hit, IWeapon weapon) {
			throw new System.NotImplementedException ();
		}

		#endregion
	}
}
