using System;
using UnityEngine;

namespace Jerre {
    [Serializable]
    public class SpawnInfo {
        public Target target;
        public float targetLifetime;
        public float nextSpawnDelay;

        public SpawnInfo() {}
        public SpawnInfo(float targetLifetime, float nextSpawnDelay, Target target) {
            this.target = target;
            this.targetLifetime = targetLifetime;
            if (nextSpawnDelay < targetLifetime + 1f) {
                this.nextSpawnDelay = targetLifetime + 1f;
                Debug.Log("Spawn delay was too low, setting it to: " + this.nextSpawnDelay);
            } else {
                this.nextSpawnDelay = nextSpawnDelay;
            }
        }
    }
}