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


    private Dictionary<int, string> mSceneDict = new Dictionary<int, string>() {
        { 0, "Main_Menu" },
        { 1, "Team_Wait_Lobby" },
        { 2, "Debug" },
        { 3, "Pillow_Fight_Wait_Lobby" },
        { 4, "Pillow_Fight" }
    };

    //public Player_Spawn spawnSpot;
    [SerializeField]
    private byte maxPlayersPerRoom = 8;

    private int chosenScene = 0;

    private void Awake() {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    private void Start() {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect(int num) {

        isConnecting = true;
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRoom(mSceneDict[chosenScene]);
            //if(!PhotonNetwork.InRoom)
            //    PhotonNetwork.JoinRandomRoom();
        } else {
            //Offline mode
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.LoadLevel(mSceneDict[chosenScene]);

        }
    }

    public void Find_Game(int sceneNum) {

        if (!Valid_Input(input.text)) {
            Debug.Log("Not valid input");
            return;
        }

        chosenScene = sceneNum;
        Connect(sceneNum);
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
        PhotonNetwork.CreateRoom(mSceneDict[chosenScene], roomOptions);
    }

    public override void OnJoinedRoom() {
        //Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        PhotonNetwork.LoadLevel(mSceneDict[chosenScene]);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom };
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(mSceneDict[chosenScene], roomOptions);
    }
}
