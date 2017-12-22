namespace Jerre
{
    public interface IWeapon
    {
        bool Fire();
        void AddListener(IHitlistener listener);
    }
}