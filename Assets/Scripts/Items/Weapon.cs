using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "Weapon File Name")]
[System.Serializable]
public class Weapon : Item //Weapon derives from item and has extra data to define a weapon
{
    public Types WeaponType;
    [Range(1,2)]
    public int Hands;
    public enum Types
    {
        Staff,
        Sword,
        Shield,
        Gun,
        Misc
    }
}
