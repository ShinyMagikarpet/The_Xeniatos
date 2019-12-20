using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum TwitchFunctionsEnum {
    PLAYER_XRAY, PLAYER_SPEED, PLAYER_FREEZE
}

public class TwitchFunctions : MonoBehaviour{

    public Dictionary<string, TwitchFunctionsEnum> dict;

    public TwitchFunctions() {

        dict = new Dictionary<string, TwitchFunctionsEnum>();
        dict.Add("Player_Xray", TwitchFunctionsEnum.PLAYER_XRAY);
        dict.Add("Player_Speed", TwitchFunctionsEnum.PLAYER_SPEED);
        dict.Add("Player_Freeze", TwitchFunctionsEnum.PLAYER_FREEZE);
    }

    public void Player_Xray() {
        List<Player> playerList = PlayerManager.Instance.Get_Players_Team1();
        foreach (Player player in playerList) {
            if (!player) return;
            player.Get_Effects_Camera().GetComponent<CameraXray>().enabled = true;
        }
    }

    public void Player_Ammo() {
        Debug.Log("Do something with player ammo");
    }

    public void Player_Speed() {
        List<Player> playerList = PlayerManager.Instance.Get_Players_Team1();
        foreach (Player player in playerList) {
            player.GetComponent<PlayerController>().speed += 2f;
        }
    }

    public void Player_Freeze() {
        List<Player> playerList = PlayerManager.Instance.Get_Players_Team1();
        foreach(Player player in playerList) {
            player.CallFreezePlayers();
        }
    }

}

