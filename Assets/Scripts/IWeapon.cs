public interface IWeapon
{
    Hit Fire();
    void AddListener(IHitlistener listener);
}