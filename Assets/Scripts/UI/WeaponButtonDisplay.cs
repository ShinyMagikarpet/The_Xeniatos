using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonDisplay : MonoBehaviour {

    public WeaponButton weaponbutton;
    public Image image;
    public Text weaponName;
    public Text ironCost;
    public Text stoneCost;
    public Text woodCost;
    private int[] recipeCost = new int[3];


    void Start(){
        recipeCost = WeaponRecipes.gWeaponRecipes[weaponbutton.mName];
        image.sprite = weaponbutton.image;
        weaponName.text = weaponbutton.mName;
        ironCost.text = "Iron: " + recipeCost[0].ToString();
        stoneCost.text = "Stone: " + recipeCost[1].ToString();
        woodCost.text = "Wood: " + recipeCost[2].ToString();

    }

}
