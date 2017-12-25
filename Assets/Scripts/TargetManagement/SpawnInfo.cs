using System;
using UnityEngine;

namespace Jerre {
    [Serializable]
    public class SpawnInfo {
        public Target target;
        public float targetLifetime;
        public float nextSpawnDelay;
		public float[] WaverDeltaAngles;
		public float[] WaverAnimationTimes;
		public bool customizeRotations = false;
		public float rotationsPerSecond;

        public SpawnInfo() {}
        public SpawnInfo(float targetLifetime, float nextSpawnDelay, Target target) {
            this.target = target;
            this.targetLifetime = targetLifetime;
            this.nextSpawnDelay = nextSpawnDelay;
        }
    }
}