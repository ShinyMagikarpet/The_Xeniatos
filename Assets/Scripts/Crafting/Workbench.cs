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
            Debug.Log(weaponName);
            targetPlayer.mPlayerWeapon.gameObject.SetActive(false);
            foreach (GameObject weaponObject in targetPlayer.mPlayerWeapons) {
                Debug.Log("Name of object is: " + weaponObject.GetComponent<Weapon>().mName);
                if (weaponObject.GetComponent<Weapon>().mName.Equals(weaponName)) {
                    targetPlayer.mPlayerWeapon = weaponObject.GetComponent<Weapon>();
                    weaponObject.SetActive(true);
                }
            }
            targetPlayer.mPlayerWeapon.Weapon_Setup();

            targetPlayer.mResourceDict["Iron"] -= WeaponRecipes.gWeaponRecipes[weaponName][0];
            targetPlayer.mResourceDict["Stone"] -= WeaponRecipes.gWeaponRecipes[weaponName][1];
            targetPlayer.mResourceDict["Wood"] -= WeaponRecipes.gWeaponRecipes[weaponName][2];
        }
        
    }
}
