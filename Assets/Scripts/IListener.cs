using UnityEngine;

namespace Jerre
{
    public interface IListener
    {
        void Notify(object notifier);
    }
}