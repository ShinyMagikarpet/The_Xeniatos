using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    public Text[] team1 = new Text[4];
    public Text[] team2  = new Text[4];
    private bool team1turn = false;

    private void Start() {
        if (PhotonNetwork.IsMasterClient) {
            team1[0].text = PhotonNetwork.NickName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        if (team1turn) {
            foreach(Text text in team1) {
                if(text.text.ToLower().Equals("looking for player...")) {
                    text.text = newPlayer.NickName;
                    team1turn = false;
                    break;
                }
            }
        }
        else {
            foreach (Text text in team2) {
                if (text.text.ToLower().Equals("looking for player...")) {
                    text.text = newPlayer.NickName;
                    team1turn = true;
                    break;
                }
            }
        }
    }

}
