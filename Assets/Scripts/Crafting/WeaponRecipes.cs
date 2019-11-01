using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecipes : MonoBehaviour
{
    //Recipe list note: int[] will always be iron -> stone -> wood
    public static Dictionary<string, int[]> gWeaponRecipes = new Dictionary<string, int[]> {
        { "Assault Rifle", new int[] { 100, 150, 100} },
        { "Confetti Gun", new int[] { 50, 100, 150} },
        { "Rainbow Flamethrower", new int[] { 100, 100, 200 } },
        { "Sniper", new int[] { 200, 200, 200 } }
    };

    public static string WeaponRecipeString(int[] recipe){

        string message = "The recipe cost is - Iron: " + recipe[0] +
            "/ Stone: " + recipe[1] + "/ Wood: " + recipe[2];


        return message;
    }
}
