using UnityEngine;

public class EmptyTrigger : ITriggerable
{
    public void AddListener(IListener listener)
    {
        
    }

    public string Name()
    {
        return "EmptyTrigger";
    }

    public void Trigger()
    {
        Debug.Log("Trigger empty trigger");
    }

    public void Notify(object notifier)
    {
        Debug.Log("Notify empty trigger");
    }
}