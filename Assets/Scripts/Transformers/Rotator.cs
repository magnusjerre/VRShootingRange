using UnityEngine;

namespace Jerre
{
    public class Rotator : MonoBehaviour
    {

        public Transform rotationTarget;
        public bool UseWorldSpace = true;
        public float RotationsPerSecond = 1f;
        public Vector3 RotationAxis = Vector3.right;

        void Awake()
        {
            if (rotationTarget == null)
            {
                rotationTarget = transform;
            }
        }

        void Update()
        {
            if (UseWorldSpace)
            {
                rotationTarget.Rotate(RotationAxis * Time.deltaTime * RotationsPerSecond * 360f, Space.World);
            }
            else
            {
                rotationTarget.Rotate(RotationAxis * Time.deltaTime * RotationsPerSecond * 360f, Space.Self);
            }
        }
    }
}