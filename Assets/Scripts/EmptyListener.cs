using UnityEngine;

public class EmptyListener : IListener
{

    private static EmptyListener instance = null;

    private EmptyListener() {

    }

    public static EmptyListener Singleton() {
        if (instance == null) {
            instance = new EmptyListener();
        }
        return instance;
    }
    public void Notify(object notifier)
    {
    }
}