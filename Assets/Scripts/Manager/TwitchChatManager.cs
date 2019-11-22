using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

public class TwitchChatManager : MonoBehaviour {

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private bool isConnected = false;
    private string sendMessagePrefix;
    private bool isVotingEventOn;
    private Dictionary<string, int> voteDict;
    public string username, password, channelName; // https://twitchapps.com/tmi

    public static TwitchChatManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake(){
        if(Instance == null) {
            Instance = this;
            isVotingEventOn = false;
            Dictionary<string, int> voteDict = new Dictionary<string, int>();
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

        if (Input.GetKeyDown(KeyCode.B)) {
            if (isVotingEventOn) {
                Debug.Log("Voting is over");
                isVotingEventOn = false;
                SendMessageToTwitch("The voting period has now ended!");
            }
            else {
                Debug.Log("Voting has started");
                isVotingEventOn = true;
                voteDict.Clear();
                SendMessageToTwitch("A New vote has started! Type \"!vote $arg (1, 2, or 3)\" to join in on the vote! \n Pancakes!");
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            PrintVotes(voteDict);
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

        string[] commands = message.Split(' ');

        if (isVotingEventOn && commands[0].ToLower().Equals("!vote")) {

            if(commands.Length > 2) {
                //SendMessageToTwitch("@" + chatname + " You have entered too many arguments for the vote o_O");
                return;
            }
            else if (voteDict.ContainsKey(chatname)) {
                SendMessageToTwitch("@" + chatname + " You have already submitted your vote :)");
                Debug.Log(chatname + " tried voting again");
                return;
            }

            int voteNum = 0;
            if(!int.TryParse(commands[1], out voteNum)){

                //SendMessageToTwitch("@" + chatname + " You can only input the given numbers to vote :)");
                return;
            }

            if(voteNum > 3 || voteNum < 1) {
                return;
            }

            Debug.Log(chatname + " has voted " + message);
            //SendMessageToTwitch(string.Format("@{0} you voted for {1} :D", chatname, commands[1]));
            voteDict.Add(chatname, voteNum);
        }
        else if (!isVotingEventOn && commands[0].ToLower().Equals("!vote")) {
            SendMessageToTwitch("@" + chatname + " there is no vote happening right now :\\");
        }
    }

    void PrintVotes(Dictionary<string, int> dict) {

        foreach (KeyValuePair<string, int> entry in dict) {
            Debug.Log(entry.Key + entry.Value);
        }

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

                VoteSystem(message, chatname);
            }
            print(message);
            if(message.Equals("PING :tmi.twitch.tv")) {
                Debug.Log("Respornding to twitch ping");
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
            }
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

    public void SendMessageToTwitch(string message, string command) {

        writer.WriteLine(sendMessagePrefix + command + ' ' + message);
        writer.Flush();
    }
}
