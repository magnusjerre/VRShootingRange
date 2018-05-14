using System;
using UnityEngine;

namespace Jerre
{
    public class MouseFire : MonoBehaviour
    {
        private MJPlayer player;

        void Start()
        {
            player = GetComponentInParent<MJPlayer>();
            if (player == null)
            {
                throw new NullReferenceException("MouseFire must have an MJPlayer parent");
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
				player.GetLeftWeapon ().Fire ();
            }
			else if (Input.GetMouseButtonDown (1)) {
				player.GetRightWeapon ().Fire ();
			}
        }
    }
}