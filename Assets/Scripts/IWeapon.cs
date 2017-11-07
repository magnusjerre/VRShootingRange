namespace Jerre
{
    public interface IWeapon
    {
        WeaponFire Fire();
        void AddListener(IHitlistener listener);
    }
}