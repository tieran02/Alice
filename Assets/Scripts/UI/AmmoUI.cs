using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour {
    //Weapon slot holder
    public Transform WeaponSlot;
    //UI text componenets
    public Text WeaponName;
    public Text WeaponAmmo;
    //Canvas group to hide the ammo
    private CanvasGroup canvasGroup;

    void Awake ()
    {
        //get the canvas group from the object
        canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //make the ammo UI invisable
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;

        //check wheather weapon slot has been assigned and is active
        if (WeaponSlot != null && WeaponSlot.childCount > 0 && WeaponSlot.GetChild(0).gameObject.activeInHierarchy)
        {
            //Get the weapon type
            GameObject weaponObject = WeaponSlot.GetChild(0).gameObject;
            Gun gun = weaponObject.GetComponent<Gun>();
            ProjectileWeapon projectileWeapon = weaponObject.GetComponent<ProjectileWeapon>();
            
            //check if the weapon is a gun, projectile weapon or melee
            if(gun != null)
            {
                //weapon is a gun
                WeaponName.text = gun.Name;
                WeaponAmmo.text = gun.CurrentAmmo + "/" + gun.MaxAmmo;
            }
            else if (projectileWeapon != null)
            {
                //weapon is a projectile weapon
                //weapon is a gun
                WeaponName.text = projectileWeapon.Name;
                WeaponAmmo.text = projectileWeapon.CurrentAmmo + "/" + projectileWeapon.MaxAmmo;
            }

            //show UI
            if (gun != null || projectileWeapon != null)
            {
                canvasGroup.alpha = 1;
            }
        }
	}
}
