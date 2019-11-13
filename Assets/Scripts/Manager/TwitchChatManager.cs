using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

public class TwitchChatManager : MonoBehaviour
{

    private TcpClient twitchClient;
    private StreamWriter reader;
    private StreamReader writer;
    private bool isConnected = false;

    public string username, password, channelName; // https://twitchapps.com/tmi

    public static TwitchChatManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake(){
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void Connect()
    {
        
    }

    public bool IsConnected() {

        return isConnected;
    }
}
