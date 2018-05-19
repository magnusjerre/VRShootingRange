using System;
using UnityEngine;

namespace Jerre { 
	[Serializable]
	public class SpawnPathInfo {
		public Target target = null;
		public float targetLifetime;
		public float timeUntilNextSpawn;
		public bool facePlayerAlways;
		public Vector3 scale = Vector3.one;

		public SpawnPathInfo() {
		}

		public static SpawnPathInfo Empty(float timeUntilNextSpawn) {
			return new SpawnPathInfo (null, 0f, timeUntilNextSpawn, false, Vector3.one);
		}

		public SpawnPathInfo(Target target, float lifetime, float nextSpawnDelay, bool facePlayerAlways, Vector3 scale) {
			this.target = target;
			targetLifetime = lifetime;
			timeUntilNextSpawn = nextSpawnDelay;
			this.facePlayerAlways = facePlayerAlways;
			this.scale = scale;
		}
	}
}
