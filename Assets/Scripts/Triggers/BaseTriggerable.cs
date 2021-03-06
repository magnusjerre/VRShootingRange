﻿using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class BaseTriggerable : MonoBehaviour, ITriggerable
    {
        [SerializeField] private string name;
        [SerializeField] private List<MonoBehaviour> mListeners;
        protected List<IListener> _listeners;

        protected void Awake()
        {
			_listeners = new List<IListener> ();
			if (mListeners != null) {
				for (var i = 0; i < mListeners.Count; i++) {
					AddListener ((IListener)mListeners [i]);
				}
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

		public void AddProgrammatically(IListener listener) {
			if (_listeners == null) {
				_listeners = new List<IListener> ();
			}
			AddListener(listener);
		}
    }
}