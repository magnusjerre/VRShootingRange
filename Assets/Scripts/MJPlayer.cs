using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class MJPlayer : MonoBehaviour
    {
        private List<IWeapon> activeWeaponList;

        private int totalScore;


        void Awake()
        {
            activeWeaponList = new List<IWeapon>();
        }

        void Start()
        {
            var weapons = GetComponentsInChildren<IWeapon>();
            foreach (var weapon in weapons)
            {
                activeWeaponList.Add(weapon);
            }
        }

        public IWeapon EquippedWeapon()
        {
            return activeWeaponList[0];
        }

    }
}