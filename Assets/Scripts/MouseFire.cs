using System;
using UnityEngine;

public class MouseFire : MonoBehaviour
{
    private IWeapon weapon;
    private MJPlayer player;

    void Start()
    {
        weapon = GetComponent<IWeapon>();
        if (weapon == null)
        {
            throw new NullReferenceException("MouseFire must have a weapon");
        }

        player = GetComponentInParent<MJPlayer>();
        if (player == null)
        {
            throw new NullReferenceException("MouseFire must have an MJPlayer parent");
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Hit result = weapon.Fire();
            if (result.IsHit)
            {
                player.AddScore(result.Score);
            }
        }
    }
}