namespace Jerre
{
    public struct WeaponFire
    {
        public bool didFire;
        public Hit hit;

        public WeaponFire(bool didFire, Hit hit)
        {
            this.didFire = didFire;
            this.hit = hit;
        }

        public static WeaponFire NoFire()
        {
            return new WeaponFire(false, Hit.Miss());
        }
    }
}