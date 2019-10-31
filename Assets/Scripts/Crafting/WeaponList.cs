using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {
    public static Dictionary<string, Weapon> gWeaponList = new Dictionary<string, Weapon>() 
    {
        {"Assault Rifle", new Assault_Rifle() },
        {"Confetti Gun", new ConfettiGun() },
        {"Rainbow Flamethrower", new Flamethrower() }
    };

}
