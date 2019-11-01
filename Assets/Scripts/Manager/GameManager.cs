using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private float matchTime = 20f * 60;
    private float timer;
    [SerializeField]private Text timeText;

    public static GameManager Instance { get; private set; }
    void Awake() {

        if (!Instance) {
            Instance = this;
        }

        if (PhotonNetwork.IsMasterClient) {
            timer = matchTime;
            timeText.text = timer.ToString();
        }else if (!PhotonNetwork.IsConnected) {
            timer = matchTime;
            timeText.text = timer.ToString();
        }
    }

    private void Update() {
        Match_Countdown_Timer();
    }

    // Update is called once per frame
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other) {

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("Send_Timer", RpcTarget.Others, timer);
    }

    [PunRPC]
    void Send_Timer(float timeIn) {
        timer = timeIn;
    }

    void Match_Countdown_Timer() {
        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            int seconds = (int)(timer) % 60;
            int minutes = (int)(timer / 60) % 60;
            string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = timerText;
        }
        else {
            Debug.Log("GAME OVER");
        }
    }

    public float Get_Timer() {
        return timer;
    }

    public float Get_Match_Time() {
        return matchTime;
    }
}
