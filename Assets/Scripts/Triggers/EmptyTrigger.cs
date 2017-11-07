using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class EmptyTrigger : ITriggerable
    {
        protected List<IListener> _listeners;

        public EmptyTrigger()
        {
            _listeners = new List<IListener>();
        }

        public void AddListener(IListener listener)
        {
            if (listener != null && !_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public string Name()
        {
            return "EmptyTrigger";
        }

        public void Trigger()
        {
            Debug.Log("Trigger empty trigger");
            for (var i = 0; i < _listeners.Count; i++)
            {
                _listeners[i].Notify(this);
            }
        }

        public void Notify(object notifier)
        {
            Debug.Log("Notify empty trigger");
            Trigger();
        }
    }
}