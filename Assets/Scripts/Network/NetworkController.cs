using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{

    public Player_Spawn spawnSpot;
    [SerializeField]
    private byte maxPlayersPerRoom = 8;
    

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
        spawnSpot = FindObjectOfType<Player_Spawn>();
    }

    void Connect() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void OnGUI() {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());    
    }

    public override void OnConnectedToMaster() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        PhotonNetwork.Instantiate("player", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
    }

    //void OnJoinedLobby() {
    //    Debug.Log("Joined lobby");
    //    PhotonNetwork.JoinRandomRoom();
    //}



    //public void OnJoinRandomFailed() {
    //    Debug.Log("Failled to join random room");
    //    PhotonNetwork.CreateRoom( null );
    //    Debug.Log("created room");
    //}

    //void OnJoinedRoom() {
    //    Debug.Log("Joined room");
    //    PhotonNetwork.Instantiate("player", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
    //}
}
