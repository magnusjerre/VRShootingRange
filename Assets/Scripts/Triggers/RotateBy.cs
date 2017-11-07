using UnityEngine;

namespace Jerre
{
    public class RotateBy : BaseTriggerable
    {
        public float time;
        public float angles;
        public Vector3 rotationAxis;

        private float totalAngles;
        private bool animate;

        void Update()
        {
            if (!animate)
            {
                return;
            }

            float speed = angles / time;
            float diffRotation = Time.deltaTime * speed;
            float rotationAmount = angles < 0 ? MinNegative(diffRotation, totalAngles, angles) : MinPositive(diffRotation, totalAngles, angles);
            transform.Rotate(rotationAxis * rotationAmount);
            totalAngles += rotationAmount;
            if (IsFinishedRotating())
            {
                animate = false;
                NotifyListeners();
            }
        }

        bool IsFinishedRotating()
        {
            if (angles < 0 && totalAngles <= angles)
            {
                return true;
            }
            if (angles >= 0 && totalAngles >= angles)
            {
                return true;
            }
            return false;
        }

        private float MinNegative(float diff, float currentSum, float targetSum)
        {
            if (diff + currentSum < targetSum)
            {
                return targetSum - currentSum;
            }
            return diff;
        }

        private float MinPositive(float diff, float currentSum, float targetSum)
        {
            if (diff + currentSum > targetSum)
            {
                return targetSum - currentSum;
            }
            return diff;
        }

        public override void Trigger()
        {
            if (animate)
            {
                Debug.Log("Already animating rotation");
                return;
            }

            animate = true;
            totalAngles = 0f;
        }

    }
}