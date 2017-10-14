using UnityEngine;

public class RotateUp : MonoBehaviour, ITriggerable
{
    public string TriggerName;
    public Transform target;
    public float animTime = 0.5f;

    [SerializeField] private float rotationAmount = -90f;
    private float totalRotation, targetRotation, initialRotation;
    private Vector3 rotationAxis = Vector3.right;
    private bool animateShow, alreadyTriggered;
    private IListener listener = EmptyListener.Singleton();

    void Start() 
    {
        if (target == null) {
            target = transform;
        }
        initialRotation = target.eulerAngles.x;
    }

    void Update()
    {
        if (animateShow)
        {
            float diff = MinPositive(-rotationAmount / animTime * Time.deltaTime, totalRotation, targetRotation);
            totalRotation += diff;
            target.Rotate(rotationAxis * diff);
            if (totalRotation >= targetRotation)
            {
                animateShow = false;
                listener.Notify(this);
            }
        }
    }

    private float MinPositive(float diff, float currentSum, float targetSum)
    {
        if (diff + currentSum > targetSum)
        {
            return targetSum - currentSum;
        }
        return diff;
    }

    public void Trigger()
    {
        if (alreadyTriggered) {
            Debug.Log("Already triggered RotateUp");
            return;
        }
        target.Rotate(rotationAxis * rotationAmount);
        totalRotation = rotationAmount;
        animateShow = true;
        targetRotation = initialRotation;
    }

    public string Name()
    {
        return TriggerName;
    }

    public void AddListener(IListener listener)
    {
        if (this.listener == EmptyListener.Singleton()) {
            this.listener = listener;
        }
    }
}