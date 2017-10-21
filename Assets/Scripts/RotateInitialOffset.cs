using UnityEngine;

public class RotateInitialOffset : MonoBehaviour, ITriggerable
{

    public string name;
    public float rotationAmount;
    public Vector3 rotationAxis;

    private IListener _listener;

    void Start()
    {
    }
    
    public void Trigger()
    {
        transform.Rotate(rotationAxis * rotationAmount);
        _listener.Notify(this);
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