public interface ITriggerable {
    void Trigger();
    string Name();
    void AddListener(IListener listener);
}