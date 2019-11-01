using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Workbench : MonoBehaviour
{

    public void Check_If_Resources(string weaponName) { //TODO: Use gameobject instead to give to player
        Make_Weapon(Player.mLocalPlayer, weaponName);
    }
    public void Make_Weapon(Player targetPlayer, string weaponName) {

        if(targetPlayer.mResourceDict["Iron"] >= WeaponRecipes.gWeaponRecipes[weaponName][0] &&
            targetPlayer.mResourceDict["Stone"] >= WeaponRecipes.gWeaponRecipes[weaponName][1] &&
            targetPlayer.mResourceDict["Wood"] >= WeaponRecipes.gWeaponRecipes[weaponName][2]) {


            for(int i = 0; i < targetPlayer.mPlayerWeapons.Length; i++) {

                GameObject weaponObject = targetPlayer.mPlayerWeapons[i];

                if (weaponObject.GetComponent<Weapon>().mName.Equals(weaponName)) {
                    if (PhotonNetwork.IsConnected)
                        targetPlayer.photonView.RPC("Player_Craft_Weapon", RpcTarget.All, i, weaponName);
                    else {

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
                        targetPlayer.mPlayerWeapon.Weapon_Setup();

                        targetPlayer.mResourceDict["Iron"] -= WeaponRecipes.gWeaponRecipes[weaponName][0];
                        targetPlayer.mResourceDict["Stone"] -= WeaponRecipes.gWeaponRecipes[weaponName][1];
                        targetPlayer.mResourceDict["Wood"] -= WeaponRecipes.gWeaponRecipes[weaponName][2];
                    }
                    
                }
            }
            

        }
        
    }
}
