using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    public InputField input;
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    private bool isConnecting;

    //public Player_Spawn spawnSpot;
    [SerializeField]
    private byte maxPlayersPerRoom = 8;

    private void Awake() {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    private void Start() {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect() {

        isConnecting = true;
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);

        if (PhotonNetwork.IsConnected) {
            Debug.Log("You are already connected");
            PhotonNetwork.JoinRandomRoom();
        } else {
            //PhotonNetwork.ConnectUsingSettings();
            
        }
    }

    public void Find_Game() {

        if (!Valid_Input(input.text)) {
            Debug.Log("Not valid input");
            return;
        }

        Connect();
        PhotonNetwork.NickName = input.text;
    }

    bool Valid_Input(string inputText) {

        if (inputText.Length > 10 || string.IsNullOrWhiteSpace(inputText)) {
            input.text = "";
            return false;
        }
        return true;
    }

    void OnGUI() {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    public override void OnConnectedToMaster() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.AutomaticallySyncScene = true;
        if (isConnecting) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        PhotonNetwork.LoadLevel("Debug");
        //PhotonNetwork.Instantiate("player", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
    }

}
