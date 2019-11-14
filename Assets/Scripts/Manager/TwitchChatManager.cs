using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

public class TwitchChatManager : MonoBehaviour
{

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private bool isConnected = false;
    private string sendMessagePrefix;
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

        if (twitchClient != null && twitchClient.Connected) {

            ReadChat();
        }

        
        
    }

    public void Connect(){

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        sendMessagePrefix = string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :", username, channelName);
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
                //message = message.Substring(splitPoint + 1);
                //print(string.Format("{0}: {1}", chatName, message));
            }
            print(message);
            if(message.Equals("PING :tmi.twitch.tv")) {
                Debug.Log("Respornding to twitch ping");
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
            }
            //Debug.Log(message);
        }
    }

    public bool IsConnected() {

        return isConnected;
    }

    public TcpClient GetTwitchClient() {
        return twitchClient;
    }

    public void SendMessageToTwitch(string message) {

        writer.WriteLine(sendMessagePrefix + message);
        writer.Flush();
    }
}
