using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre {
	public class ChainHitsSpawner : MonoBehaviour, IListener, ITargetSpawner {

		[SerializeField] private SpawnInfoConsecutive[] spawnInfo;
		[SerializeField] private float startDelay;
		[SerializeField] private int repeat = -1;

		private bool spawningOngoing = false;
		private int currentSpawnIndex;
		private int currentSpawnInfoIndex;
		private int repeatsLeft;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		}

		#region IListener implementation

		public void Notify (object notifier)
		{
			var target = ((BaseTriggerable)notifier).GetComponentInParent<Target> ();
			var ps = GetParticleSystemIndicatorForTarget (target);
			if (ps != null) {
				ps.Stop ();
			}

			if (target.IsDestroyed && target.TotalDestructionScore > 0 && !IsLastTargetInChain()) {
				Spawn ();
			} else {
				repeatsLeft--;
				if (repeatsLeft != 0) {
					Reset ();
					Invoke ("Spawn", startDelay);
				}
			}
		}

		private ParticleSystem GetParticleSystemIndicatorForTarget(Target target) {
			var parent = target.transform.parent;
			for (var i = 0; i < parent.childCount; i++) {
				var particleSystem = parent.GetChild (i).GetComponent<ParticleSystem>();
				if (particleSystem != null) {
					return particleSystem;
				}
			}
			return null;
		}

		private bool IsLastTargetInChain() {
			return currentSpawnIndex == transform.childCount - 1;
		}

		#endregion

		#region ITargetSpawner implementation

		public void StartSpawningProcess ()
		{
			spawningOngoing = true;
			Reset ();
			Invoke ("Spawn", startDelay);
		}

		public void EndSpawningProcess ()
		{
			spawningOngoing = false;
		}

		#endregion

		public void Spawn() {
			if (!spawningOngoing) {
				return;
			}

			currentSpawnIndex = NextSpawnIndex ();
			currentSpawnInfoIndex = NextSpawnInfoIndex ();
			var spawnInfo = this.spawnInfo [currentSpawnInfoIndex];
			var spawnPoint = transform.GetChild (currentSpawnIndex);
			var target = Instantiate (spawnInfo.target, spawnPoint);
			target.HideTrigger.AddListener (this);
			if (spawnInfo.particleSystemIndicator != null) {
				var ps = Instantiate (spawnInfo.particleSystemIndicator, spawnPoint);
				var main = ps.main;
				main.stopAction = ParticleSystemStopAction.Destroy;
				ps.Play ();
			}
		}

		private int NextSpawnIndex() {
			return (currentSpawnIndex + 1) % transform.childCount;
		}

		private int NextSpawnInfoIndex() {
			return (currentSpawnInfoIndex + 1) % spawnInfo.Length;
		}

		private void Reset() {
			repeatsLeft = repeat;
			currentSpawnIndex = -1;
			currentSpawnInfoIndex = -1;
		}
	}
}
