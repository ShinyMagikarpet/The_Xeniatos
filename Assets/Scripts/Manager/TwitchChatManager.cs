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
    private bool isVotingEventOn = false;
    private Dictionary<string, int> voteDict;
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

        if (Input.GetKeyDown(KeyCode.V)) {
            if (isVotingEventOn) {
                Debug.Log("Voting is over");
                isVotingEventOn = false;
            }
            else {
                Debug.Log("Voting has started");
                isVotingEventOn = true;
                voteDict = new Dictionary<string, int>();
            }
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

    void VoteSystem(string message, string chatname) {

    }

    void ReadChat() {

        if(twitchClient.Available > 0) {

            var message = reader.ReadLine();

            if (message.Contains("PRIVMSG")) {

                //var splitPoint = message.IndexOf('!', 1);
                //var chatName = message.Substring(0, splitPoint);
                //chatName = chatName.Substring(1);

                string chatname = GetUsernameFromTwitch(message);
                Debug.Log(chatname + " sent this message");

                int splitPoint = message.IndexOf(':', 1);
                message = message.Substring(splitPoint + 1);

                if (isVotingEventOn && message.Substring(0, 5).Equals("!vote")) {
                    Debug.Log(chatname + " has voted");
                    SendMessageToTwitch(string.Format("@{0} you voted for {1}", chatname, message));
                }
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

    string GetUsernameFromTwitch(string message) {

        int splitPoint = message.IndexOf('!', 1);
        string chatName = message.Substring(0, splitPoint);
        chatName = chatName.Substring(1);
        return chatName;
    }

    public TcpClient GetTwitchClient() {
        return twitchClient;
    }

    public void SendMessageToTwitch(string message) {

        writer.WriteLine(sendMessagePrefix + message);
        writer.Flush();
    }
}
