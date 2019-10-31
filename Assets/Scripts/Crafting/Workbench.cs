using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workbench : MonoBehaviour
{

    public void Check_If_Resources(string weaponName) { //TODO: Use gameobject instead to give to player
        Make_Weapon(Player.mLocalPlayer, weaponName);
    }
    public void Make_Weapon(Player targetPlayer, string weaponName) {

        if(targetPlayer.mResourceDict["Iron"] >= WeaponRecipes.gWeaponRecipes[weaponName][0] &&
            targetPlayer.mResourceDict["Stone"] >= WeaponRecipes.gWeaponRecipes[weaponName][1] &&
            targetPlayer.mResourceDict["Wood"] >= WeaponRecipes.gWeaponRecipes[weaponName][2]) {

            foreach (GameObject weaponObject in targetPlayer.mPlayerWeapons) {

                if (weaponObject.GetComponent<Weapon>().mName.Equals(weaponName)) {

                    if (targetPlayer.mPlayerSubWeapon == null) {
                        targetPlayer.mPlayerSubWeapon = weaponObject.GetComponent<Weapon>();
                        weaponObject.SetActive(true);
                        targetPlayer.Player_Switch_Weapons();
                    }
                    else {
                        targetPlayer.mPlayerWeapon.gameObject.SetActive(false);
                        targetPlayer.mPlayerWeapon = weaponObject.GetComponent<Weapon>();
                        weaponObject.SetActive(true);
                    }
                }
            }
            targetPlayer.mPlayerWeapon.Weapon_Setup();

            targetPlayer.mResourceDict["Iron"] -= WeaponRecipes.gWeaponRecipes[weaponName][0];
            targetPlayer.mResourceDict["Stone"] -= WeaponRecipes.gWeaponRecipes[weaponName][1];
            targetPlayer.mResourceDict["Wood"] -= WeaponRecipes.gWeaponRecipes[weaponName][2];

        }
        
    }
}
