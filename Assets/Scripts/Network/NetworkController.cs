using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //connect to server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        //base.OnConnectedToMaster();
        Debug.Log("You have connected to " + PhotonNetwork.CloudRegion + " server");
    }

    void Update()
    {
        
    }
}
