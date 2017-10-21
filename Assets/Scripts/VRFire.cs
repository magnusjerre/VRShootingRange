using System;
using UnityEngine;

public class VRFire : MonoBehaviour
{
    private SteamVR_TrackedObject _trackedObject;
    private SteamVR_Controller.Device device;
    private IWeapon weapon;

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
            weapon.Fire();
        }
    }

    bool IsReleasingTrigger()
    {
        return device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
    }
}