using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum TwitchFunctionsEnum {
    PLAYER_HEALTH, PLAYER_AMMO, PLAYER_SPEED, PLAYER_FREEZE
}

public class TwitchFunctions : MonoBehaviour{

    public Dictionary<string, TwitchFunctionsEnum> dict;

    public TwitchFunctions() {

        dict = new Dictionary<string, TwitchFunctionsEnum>();
        dict.Add("Player_Health", TwitchFunctionsEnum.PLAYER_HEALTH);
        dict.Add("Player_Ammo", TwitchFunctionsEnum.PLAYER_AMMO);
        dict.Add("Player_Speed", TwitchFunctionsEnum.PLAYER_SPEED);
        dict.Add("Player_Freeze", TwitchFunctionsEnum.PLAYER_FREEZE);
    }

    public void Player_Health() {
        Debug.Log("Do something with player health and the number ");
    }

    public void Player_Ammo() {
        Debug.Log("Do something with player ammo");
    }

    public void Player_Speed() {
        Debug.Log("Do something with player speed");
    }

    public void Player_Freeze() {
        Debug.Log("Freeze a player");
    }

}

