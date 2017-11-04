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
            this.nextSpawnDelay = nextSpawnDelay;
        }
    }
}