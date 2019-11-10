using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{

    public GameObject mPlayerPrefab;

    private void Start() {
        
        if(mPlayerPrefab == null) {
            Debug.LogError("Need to place player prefab in this instance of the script", this);
        } 
        else {
            if (Player.LocalPlayerInstance == null) {

                PhotonNetwork.Instantiate(mPlayerPrefab.name, Vector3.zero, Quaternion.identity);
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        //PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void Leave_Room() {

        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other) {

        Debug.Log(other.NickName + " has joined the game");
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player other) {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    private void Update() {
        if(PhotonNetwork.InRoom)
            Debug.Log("There are " + PhotonNetwork.CurrentRoom.PlayerCount + " in this room");

    } 

    /*
    void LoadArena() {

        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }

        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);

    }
    */

}
