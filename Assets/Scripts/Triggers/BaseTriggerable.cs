using System.Collections.Generic;
using UnityEngine;

public class BaseTriggerable : MonoBehaviour, ITriggerable
{
    [SerializeField] private string name;
    [SerializeField] private List<MonoBehaviour> mListeners;
    protected List<IListener> _listeners;

    void Awake()
    {
        _listeners = new List<IListener>();
        for (var i = 0; i < mListeners.Count; i++)
        {
            AddListener((IListener) mListeners[i]);
        }
    }
    
    public void Notify(object notifier)
    {
        Trigger();
    }

    public virtual void Trigger()
    {
        Debug.Log("Defualt trigger implementation");
    }

    public string Name()
    {
        return name;
    }

    public void AddListener(IListener listener)
    {
        if (listener != null && !_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void NotifyListeners()
    {
        for (var i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].Notify(this);
        }
    }
}