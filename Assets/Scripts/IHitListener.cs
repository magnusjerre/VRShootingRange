namespace Jerre
{
    public interface IHitlistener
    {
        void NotifyHit(Hit hit, IWeapon weapon);
    }
}