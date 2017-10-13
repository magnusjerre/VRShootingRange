using UnityEngine;

public class RotateDown : MonoBehaviour, ITriggerable
{
    public string TriggerName;
    public Transform target;
    public float animHideTime = 0.25f;

    [SerializeField] private float rotationAmount = -90f;
    private float totalRotation, targetRotation;
    private bool animate, alreadyTriggered = false;

    public bool DestroyOnFinish = true;

    void Start() {
        if (target == null) {
            target = transform;
        }
    }

    void Update()
    {
        if (animate)
        {
            float diff = rotationAmount / animHideTime * Time.deltaTime;
            totalRotation += diff;
            target.Rotate(Vector3.right * diff);
            if (totalRotation <= targetRotation)
            {
                animate = false;
                if (DestroyOnFinish) {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Trigger()
    {
        if (alreadyTriggered) {
            Debug.Log("Already triggered RotateUp");
            return;
        }
        alreadyTriggered = true;
        animate = true;
        targetRotation = target.localEulerAngles.x + rotationAmount;
    }

    public string Name()
    {
        return TriggerName;
    }
}