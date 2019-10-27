using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workbench : MonoBehaviour
{

    public void Check_If_Resources(string weaponName) {
        Make_Weapon(Player.mLocalPlayer, weaponName);
    }
    public void Make_Weapon(Player targetPlayer, string weaponName) {

        if(targetPlayer.mResourceDict["Iron"] >= WeaponRecipes.gWeaponRecipes[weaponName][0] &&
            targetPlayer.mResourceDict["Stone"] >= WeaponRecipes.gWeaponRecipes[weaponName][1] &&
            targetPlayer.mResourceDict["Wood"] >= WeaponRecipes.gWeaponRecipes[weaponName][2]) {

            targetPlayer.mPlayerWeapon = WeaponList.gWeaponList[weaponName];
            targetPlayer.mResourceDict["Iron"] -= WeaponRecipes.gWeaponRecipes[weaponName][0];
            targetPlayer.mResourceDict["Stone"] -= WeaponRecipes.gWeaponRecipes[weaponName][1];
            targetPlayer.mResourceDict["Wood"] -= WeaponRecipes.gWeaponRecipes[weaponName][2];
        }
        
    }
}
