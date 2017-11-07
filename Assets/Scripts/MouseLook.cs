using UnityEngine;

namespace Jerre
{
    public class MouseLook : MonoBehaviour
    {

        public float VerticalMouseSensitivity = 5;
        public float HorizontalMouseSensitivity = 3;
        public bool LookHorizonal = false;

        public float maxVerticalRotation = 30f;
        public float minVerticalRotation = -30f;
        public float currentVerticalRotation = 0f;

        void Start()
        {
            currentVerticalRotation = transform.localEulerAngles.x;
            Cursor.visible = false;
        }

        void Update()
        {

            if (LookHorizonal)
            {
                float yRotation = Input.GetAxis("Mouse X") * HorizontalMouseSensitivity;
                transform.Rotate(0, yRotation, 0);
            }
            else
            {
                float xDeltaRotation = Input.GetAxis("Mouse Y") * VerticalMouseSensitivity;
                currentVerticalRotation += xDeltaRotation;
                currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minVerticalRotation, maxVerticalRotation);
                transform.localEulerAngles = new Vector3(-currentVerticalRotation, 0, 0);
            }
        }
    }

}