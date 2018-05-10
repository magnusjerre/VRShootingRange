using System;
using UnityEngine;

namespace Jerre
{
    public struct Hit
    {
		public HitEnum hitType;
		public bool IsHit { get { return hitType == HitEnum.HIT; } }
		public bool IsMiss { get { return hitType == HitEnum.MISS; } }
        public int Score;

        public Vector3 HitLocation;

		public Hit(HitEnum hitType, int score, Vector3 hitLocation)
        {
			this.hitType = hitType;
            Score = score;
            HitLocation = hitLocation;
        }

        public override string ToString()
        {
			return String.Format("{hitType: {0}, score: {1}}", hitType, Score);
        }

        public static Hit Miss()
        {
			return new Hit(HitEnum.MISS, 0, Vector3.zero);
        }

		public static Hit GameStateChange() 
		{
			return new Hit (HitEnum.GAME_STATE_CHANGE, 0, Vector3.zero);
		}
    }
}