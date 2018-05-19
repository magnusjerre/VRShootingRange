namespace Jerre
{
    public interface IWeapon
    {
        bool Fire();
        void AddHitListener(IHitlistener listener);
		void AddFireListener(IFireListener listener);
		void ResetCooldown();
    }
}