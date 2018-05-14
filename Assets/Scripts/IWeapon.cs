namespace Jerre
{
    public interface IWeapon
    {
        bool Fire();
        void AddListener(IHitlistener listener);
		void AddFireListener(IFireListener listener);
		void ResetCooldown();
    }
}