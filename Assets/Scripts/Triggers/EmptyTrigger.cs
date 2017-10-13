using UnityEngine;

public class EmptyTrigger : ITriggerable
{
    public string Name()
    {
        return "EmptyTrigger";
    }

    public void Trigger()
    {
        Debug.Log("Trigger empty trigger");
    }
}