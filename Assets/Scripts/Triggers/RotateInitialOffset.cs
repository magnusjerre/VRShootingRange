using UnityEngine;

public class RotateInitialOffset : BaseTriggerable
{
    public float rotationAmount;
    public Vector3 rotationAxis;
    
    void Start()
    {
    }
    
    public override void Trigger()
    {
        transform.Rotate(rotationAxis * rotationAmount);
        NotifyListeners();
    }

}