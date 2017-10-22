public interface ITriggerable : IListener {
    void Trigger();
    string Name();
    void AddListener(IListener listener);
}