using System;
using UnityEngine;

namespace Jerre
{
    public class VRFire : MonoBehaviour
    {
        private SteamVR_TrackedObject _trackedObject;
        private SteamVR_Controller.Device device;
        private IWeapon weapon;
        public float vibrationTime;
        private float elapsedTime;
        private bool vibrate;

        void Awake()
        {
            if (weapon == null)
            {
                weapon = GetComponent<IWeapon>();
            }

            if (weapon == null)
            {
                weapon = GetComponentInChildren<IWeapon>();
            }

            if (weapon == null)
            {
                throw new NullReferenceException("Must have a weapon reference");
            }
        }

        void OnEnable()
        {
            _trackedObject = GetComponent<SteamVR_TrackedObject>();

        }

        void Update()
        {
            device = SteamVR_Controller.Input((int)_trackedObject.index);
            if (IsReleasingTrigger())
            {
                if (weapon.Fire())
                {
                    vibrate = true;
                }
            }
            if (vibrate)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= vibrationTime)
                {
                    vibrate = false;
                    elapsedTime = 0f;
                }
                device.TriggerHapticPulse(3999);
            }
        }

        bool IsReleasingTrigger()
        {
            return device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
        }
    }
}