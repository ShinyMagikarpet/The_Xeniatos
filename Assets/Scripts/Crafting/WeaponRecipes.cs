using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecipes : MonoBehaviour
{

    public static Dictionary<string, int[]> gWeaponRecipes;


    static WeaponRecipes(){

        gWeaponRecipes = new Dictionary<string, int[]>();
        gWeaponRecipes.Add("Assault Rifle", new int[] { 100, 150, 100});

    }

    ~WeaponRecipes() { }

    public static string WeaponRecipeString(int[] recipe){

        string message = "The recipe cost is - Iron: " + recipe[0] +
            "/ Stone: " + recipe[1] + "/ Wood: " + recipe[2];


        return message;
    }
}
