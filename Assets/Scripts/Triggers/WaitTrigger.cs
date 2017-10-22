using UnityEngine;

public class WaitTrigger : MonoBehaviour, ITriggerable
{
    public float waitTime;
    [SerializeField] private string name;
    private IListener _listener;
    private bool animate;

    void Start()
    {
        
    }

    private void NotifyListeners()
    {
        if (_listener != null)
        {
            _listener.Notify(this);
        }
        animate = false;
    }
    
    public void Trigger()
    {
        Invoke("NotifyListeners", waitTime);
        animate = true;
    }

    public string Name()
    {
        return name;
    }

    public void AddListener(IListener listener)
    {
        _listener = listener;
    }
}