using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]private List<Player> players = new List<Player>();
    [SerializeField] private List<Player> team1Players = new List<Player>();
    [SerializeField] private List<Player> team2Players = new List<Player>();
    public static PlayerManager Instance { get; private set; }

    void Awake() {
        if (!Instance) {
            Instance = this;
        }
    }

    private void Update() {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList) {
            GameObject playerObj = player.TagObject as GameObject;
            if (playerObj != null) {
                Debug.Log("Object is " + playerObj.name);
            }
            //Debug.Log(player.NickName);
        }
    }

    public void Add_Player(Player player) {

        players.Add(player);
    }

    public void Remove_Player(Player player) {

        players.Remove(player);
    }

    public void Put_Player_On_Team(Player player) {

        if(player.playerTeamNum == 1) {
            team1Players.Add(player);
        }
        else {
            team2Players.Add(player);
        }
    }

    public Player Get_Player(GameObject playerObj) {

        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList) {

            if(player.TagObject as GameObject == playerObj) {

            }
        }

        return null;
    }

}
