using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks{
    public byte teamDifference;
    public Text[] playerNames = new Text[8];
    public Text[] team1;
    public Text[] team2;
    private byte team1Count = 0;
    private byte team2Count = 0;

    private byte playerCount = 0;
    [SerializeField] private byte nextLevelIndex;
    private void Start() {
        int i;
        team1 = new Text[8 - teamDifference];
        for(i = 0; i < team1.Length; i++) {
            team1[i] = playerNames[i];
        }
        team2 = new Text[8 - team1.Length];
        for(i = 0; i < team2.Length; i++) {
            team2[i] = playerNames[8 - team2.Length + i];
        }
        if (PhotonNetwork.IsMasterClient) {
            //master will always be first to lobby
            team1[0].text = PhotonNetwork.NickName;
            team1Count++;
            playerCount++;
            PhotonNetwork.LocalPlayer.TagObject = 1;
            Start_Game(nextLevelIndex);
        }
    }

    private void Update() {
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList) {
            Debug.Log(player.NickName + " " + player.TagObject);
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient) {
        //If we are switching master clients, we should pass values to new master from old to continue matchmaking
        base.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        if (PhotonNetwork.IsMasterClient)
        {

            if (team1Count == team2Count || team1Count < team2Count)
            {
                foreach (Text text in team1)
                {
                    if (text.text.ToLower().Equals("looking for player..."))
                    {
                        text.text = newPlayer.NickName;
                        break;
                    }
                }
                newPlayer.TagObject = 1;
                team1Count++;
            }
            else
            {
                foreach (Text text in team2)
                {
                    if (text.text.ToLower().Equals("looking for player..."))
                    {
                        text.text = newPlayer.NickName;
                        break;
                    }
                }
                newPlayer.TagObject = 2;
                team2Count++;
            }

            string[] team1Names = Get_Names_From_Text(team1);
            string[] team2Names = Get_Names_From_Text(team2);
            photonView.RPC("Send_Team_Names", RpcTarget.Others, team1Names, team2Names);
            playerCount++;
        }

        //if (playerCount >= 2) {
        //    Start_Game(nextLevelIndex);
        //}

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        for(int i = 0; i < team1.Length; i++)
        {
            if (team1[i].text.Equals(otherPlayer.NickName))
            {
                team1[i].text = "Looking for player...";
                team1Count--;
                break;
            }

            if (team2[i].text.Equals(otherPlayer.NickName))
            {
                team2[i].text = "Looking for player...";
                team2Count--;
                break;
            }
        }

        if (PhotonNetwork.IsMasterClient) {
            playerCount--;

            Debug.Log("Player left called");
        }

    }

    private string[] Get_Names_From_Text(Text[] teamNames){
        string[] names = new string[teamNames.Length];
        int i = 0;
        foreach(Text textUI in teamNames){
            names[i] = textUI.text;
            i++;
        }

        return names;
    }

    public override void OnLeftRoom(){
        base.OnLeftRoom();
        SceneManager.LoadScene(0);

    }

    public void Leave_Room(){
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC("Send_New_Master_Data", RpcTarget.Others, team1Count, team2Count, playerCount);
        }
        PhotonNetwork.LeaveRoom();
    }

    public void Start_Game(int levelNum) {
        if (!PhotonNetwork.IsMasterClient)
            return;
        StartCoroutine(Game_Start_Timer(2f, levelNum));
        
    }

    IEnumerator Game_Start_Timer(float timer, int levelNum) {
        yield return new WaitForSeconds(timer);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(levelNum);
    }

    [PunRPC]
    private void Send_Team_Names(string[] team1Names, string[] team2Names){

        for(int i = 0; i < team1Names.Length; i++){
            team1[i].text = team1Names[i];
            team2[i].text = team2Names[i];
        }
    }

    [PunRPC]
    private void Send_New_Master_Data(byte team1count, byte team2count, byte totalcount) {
        team1Count = team1count;
        team2Count = team2count;
        playerCount = totalcount;
    }



}
