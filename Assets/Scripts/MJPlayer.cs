using System.Collections.Generic;
using UnityEngine;
using Jerre;

namespace Jerre
{
	public class MJPlayer : MonoBehaviour
    {
        private List<IWeapon> activeWeaponList;

		public IWeapon left, right;

        private int totalScore;


        void Awake()
        {
            activeWeaponList = new List<IWeapon>();
			FindObjectOfType<GameController> ().mjPlayer = this;
        }

        void Start()
        {
            var weapons = GetComponentsInChildren<IWeapon>();
            foreach (var weapon in weapons)
            {
                activeWeaponList.Add(weapon);
				if (((Weapon) weapon).position == WeaponEnum.LEFT) {
					left = weapon;
				}
				else 
				{
					right = weapon;
				}
            }
        }

        public IWeapon EquippedWeapon()
        {
            return activeWeaponList[0];
        }

		public IWeapon GetLeftWeapon() 
		{
			return left;
		}

		public IWeapon GetRightWeapon() 
		{
			return right;
		}

		public IWeapon GetOtherWeapon(IWeapon weapon) {
			return weapon == left ? right : left;
		}
    }
}