using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {
    public static Dictionary<string, Weapon> gWeaponList = new Dictionary<string, Weapon>();

    static WeaponList(){
        gWeaponList.Add("Assault Rifle", new Flamethrower());
        gWeaponList.Add("Confetti Gun", new ConfettiGun());
    }
}
