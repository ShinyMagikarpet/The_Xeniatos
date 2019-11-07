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

                
                PhotonNetwork.Instantiate(mPlayerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);
                //Player player = playerObj.GetComponent<Player>();
                //if (player) {
                //    PlayerManager.Instance.Add_Player(player);
                //    playerObj.transform.SetParent(PlayerManager.Instance.transform);
                //}
                //TODO: Make function to set player team from tagobject and then set tagobject to actual player object
                //Debug.Log("Player team is " + PhotonNetwork.LocalPlayer.TagObject);
                //PhotonNetwork.LocalPlayer.TagObject = playerObj;
                //GameObject gameObject = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
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

    /*
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
    */
}
