using System;

namespace Jerre
{
    [Serializable]
    public struct MultiplierStreak
    {
        public int requiredStreak;
        public float multiplier;

        public MultiplierStreak(int requiredStreak, float multiplier)
        {
            this.requiredStreak = requiredStreak;
            this.multiplier = multiplier;
        }

        public override string ToString()
        {
            return string.Format("[requiredStreak: {0}, multiplier: {1}]", requiredStreak, multiplier);
        }
    }
}