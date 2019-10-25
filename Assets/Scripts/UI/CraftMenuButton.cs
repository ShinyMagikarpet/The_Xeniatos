using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMenuButton : MonoBehaviour
{
    public PlayerUI playerUI;

    //I hate this work around!!!
    public void Use_Craft_Menu() {
        playerUI.Use_Craft_Menu();
    }
}
