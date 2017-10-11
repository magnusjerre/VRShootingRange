using UnityEngine;

public class TargetLifecycle : MonoBehaviour
{
    public Transform target;
    public float animShowTime = 0.5f, animHideTime = 0.25f;

    private float rotationAmount = -90f, totalRotation;
    private float targetRotation;
    private Vector3 rotationAxis = Vector3.right;
    private bool animateShow, animateHide;
    private float initalRotation;

    void Awake()
    {
        initalRotation = transform.eulerAngles.x;
    }

    void Start()
    {
        target.Rotate(rotationAxis * rotationAmount);
        totalRotation = rotationAmount;
        Show();
    }

    void Update()
    {
        if (animateShow)
        {
            float diff = MinPositive(-rotationAmount / animShowTime * Time.deltaTime, totalRotation, targetRotation);
            totalRotation += diff;
            target.Rotate(rotationAxis * diff);
            Debug.Log("Heio");
            if (totalRotation >= targetRotation)
            {
                animateShow = false;
            }
        }
        else if (animateHide)
        {
            float diff = MinNegative(rotationAmount / animHideTime * Time.deltaTime, totalRotation, targetRotation);
            totalRotation += diff;
            target.Rotate(rotationAxis * (rotationAmount / animHideTime) * Time.deltaTime);
            if (totalRotation <= targetRotation)
            {
                animateHide = false;
            }
        }
    }

    public void Show()
    {
        if (!animateHide)
        {
            animateShow = true;
            targetRotation = initalRotation;
            Debug.Log("targetRotation: " + targetRotation + "; totalRotation: " + totalRotation);
        }
    }

    public void Hide()
    {
        if (!animateShow)
        {
            animateHide = true;
            targetRotation = initalRotation + rotationAmount;
        }
    }

    private float MinNegative(float diff, float currentSum, float targetSum)
    {
        return diff;
//        if (diff + currentSum < targetSum)
//        {
//            return currentSum - targetSum;
//        }
//        return diff;
    }

    private float MinPositive(float diff, float currentSum, float targetSum)
    {
        if (diff + currentSum > targetSum)
        {
            return targetSum - currentSum;
        }
        return diff;
    }
}