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
    public Text timeText;
    public GameObject sun;

    // Start is called before the first frame update
    void Awake() {
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
            sun.transform.Rotate(0.008f, 0, 0);
        }
        else {
            Debug.Log("GAME OVER");
        }
    }
}
