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

    public string username, password, channelName; // https://twitchapps.com/tmi



    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void Connect()
    {
        
    }
}
