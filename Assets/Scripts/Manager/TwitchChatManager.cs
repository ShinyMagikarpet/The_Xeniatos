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
    private StreamReader reader;
    private StreamWriter writer;
    private bool isConnected = false;

    public string username, password, channelName; // https://twitchapps.com/tmi

    public static TwitchChatManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake(){
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Connect();
        }
        else {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update(){

        if (!twitchClient.Connected) {
                
            Connect();
        }

        ReadChat();
        
    }

    private void Connect(){

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);

        writer.Flush();
        
    }

    void ReadChat() {

        if(twitchClient.Available > 0) {

            var message = reader.ReadLine();

            if (message.Contains("PRIVMSG")) {

                var splitPoint = message.IndexOf('!', 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(':', 1);
                message = message.Substring(splitPoint + 1);
                print(string.Format("{0}: {1}", chatName, message));
            }
            print(message);
            //Debug.Log(message);
        }
    }

    public bool IsConnected() {

        return isConnected;
    }
}
