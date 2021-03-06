﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGS
{
    /// <summary>
    /// This class is responsible for using/swapping player weapons.
    /// </summary>
    public class WeaponSelector : MonoBehaviour
    {
        //=====================================================================
        #region Instance variables
        //=====================================================================
        /// <summary>
        /// The index of the selected weapon.
        /// </summary>
        private int _selectedWeaponIndex = 0;
        #endregion

        //=====================================================================
        #region Properties
        //=====================================================================
        /// <summary>
        /// The index of the selected weapon.
        /// </summary>
        public bool SingleFire
        {
            get => transform.GetChild(_selectedWeaponIndex).GetComponent<AWeapon>().SingleFire;
            set => transform.GetChild(_selectedWeaponIndex).GetComponent<AWeapon>().SingleFire = value;
        }
        /// <summary>
        /// The selected weapon.
        /// </summary>
        public AWeapon SelectedWeapon
        {
            get => transform.GetChild(_selectedWeaponIndex).GetComponent<AWeapon>();
        }
        #endregion

        //=====================================================================
        #region MonoBehaviour
        //=====================================================================
        /// <summary>
        /// Called before the first frame update.
        /// </summary>
        private void Start()
        {
            SelectWeapon(_selectedWeaponIndex);
        }
        #endregion

        //=====================================================================
        #region Public methods
        //=====================================================================
        /// <summary>
        /// Uses the currently selected weapon.
        /// </summary>
        /// <param name="direction">
        /// The direction of the attack.
        /// </param>
        public void UseSelectedWeapon(Vector3 direction)
        {
            if (HasWeapon())
            {
                transform.GetChild(_selectedWeaponIndex).GetComponent<AWeapon>().Attack(direction);
            }
        }

        /// <summary>
        /// Increments the weapon index by 1 and selects the new weapon. Wraps
        /// around if at end of the array.
        /// </summary>
        public void IncrementWeaponIndex()
        {
            if (HasMultipleWeapons())
            {
                // Increment index (with wrap around)
                SelectWeapon((_selectedWeaponIndex + 1) % transform.childCount);
            }
        }

        /// <summary>
        /// Decrements the weapon index by 1 and selects the new weapon. Wraps
        /// around if at beginning of the array.
        /// </summary>
        public void DecrementWeaponIndex()
        {
            if (HasMultipleWeapons())
            {
                // Decrement index (with wrap around)
                SelectWeapon((_selectedWeaponIndex + transform.childCount - 1) % transform.childCount);
            }
        }
        #endregion

        //=====================================================================
        #region Private methods
        //=====================================================================
        /// <summary>
        /// Activates the child GameObject corresponding to the selected weapon
        /// index. Deactivates all other children.
        /// </summary>
        private void SelectWeapon(int selectedWeaponIndex)
        {
            if (SelectedWeapon is BulletDWGOWeapon)
            {
                BulletDWGOWeapon weapon = (BulletDWGOWeapon)SelectedWeapon;
                weapon.isReloading = false;
            }
            _selectedWeaponIndex = selectedWeaponIndex;
            if (HasWeapon())
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (i == _selectedWeaponIndex)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }

                }
                EventManager.Instance.Invoke(EventName.WeaponSwitchEvent,
                    new WeaponSwitchEventArgs(),
                    this);
            }
        }

        /// <summary>
        /// Checks if the player is currently holding a weapon.
        /// </summary>
        /// <returns>
        /// True if player is holding at least one weapon, false otherwise.
        /// </returns>
        private bool HasWeapon()
        {
            return transform.childCount > 0;
        }

        /// <summary>
        /// Checks if the player is currently holding multiple weapons.
        /// </summary>
        /// <returns>
        /// True if player is holding at least two weapons, false otherwise.
        /// </returns>
        private bool HasMultipleWeapons()
        {
            return transform.childCount > 1;
        }
        #endregion
    }
}
