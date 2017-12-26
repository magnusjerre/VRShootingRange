using System;
using UnityEngine;

namespace Jerre {
	[Serializable]
	public class SpawnInfoConsecutive
	{
		public Target target;
		public float lifetime;
		public ParticleSystem particleSystemIndicator;

		public SpawnInfoConsecutive() {}
	}
}

