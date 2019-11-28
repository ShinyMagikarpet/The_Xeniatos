using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using System.Reflection;

public class TwitchChatManager : MonoBehaviour {

    public enum TwitchState { IDLE, VOTE_BEGIN, VOTING, VOTE_ENDING };

    public GameObject twitchFunctionsPrefab;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private bool isConnected = false;
    private string sendMessagePrefix;
    private bool isVotingEventOn = false;
    [SerializeField]private TwitchState state = TwitchState.IDLE;
    private float votingTimer = -1f;
    private float messageDelay = 0f;
    private float remindTimer;
    private TwitchFunctions twitchFunctions;
    string[] functions;
    private Dictionary<string, int> voteDict;
    public string username, password, channelName; // https://twitchapps.com/tmi
    
    public static TwitchChatManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake(){
        if(Instance == null) {
            Instance = this;
            isVotingEventOn = false;
            voteDict = new Dictionary<string, int>();
            twitchFunctions = twitchFunctionsPrefab.GetComponent<TwitchFunctions>();
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
        
    }

    // Update is called once per frame
    void Update() { 

        if (twitchClient != null && twitchClient.Connected) {

            ReadChat();
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            Set_Timer(60f);
            remindTimer = votingTimer / 4;
        }

        if(votingTimer > 0) {
            isVotingEventOn = true;
            votingTimer -= Time.deltaTime;
            if (state != TwitchState.VOTING) {
                state = TwitchState.VOTE_BEGIN;
                messageDelay = 2f;
            }

            if(Time.time > messageDelay && (Mathf.CeilToInt(votingTimer)) % (int)remindTimer == 0 && Mathf.CeilToInt(votingTimer) != 0) {

                messageDelay = Time.time + 2f;
                SendMessageToTwitch("Time remaining for the vote:", Mathf.CeilToInt(votingTimer));
            }
            

        }
        else if(votingTimer < 0f && votingTimer > -1f){
            isVotingEventOn = false;
            state = TwitchState.VOTE_ENDING;
            votingTimer = -1f;
        }
        else {
            state = TwitchState.IDLE;
        }

        if (twitchClient != null) {

            if (!isVotingEventOn && state == TwitchState.VOTE_ENDING) {
                SendMessageToTwitch("The voting period has now ended!");
                GetResults();
                voteDict.Clear();
            }
            else if(isVotingEventOn && state == TwitchState.VOTE_BEGIN){
                SendMessageToTwitch("A New vote has started! Type \"!vote $arg (1, 2, or 3)\" to join in on the vote!", Mathf.CeilToInt(votingTimer));
                functions = GetTwitchFunctionNames();
                
                SendMessageToTwitch(string.Format("Avaiable Voting Options! 1. {0}                   2. {1}                              3. {2}",
                    functions[0],
                    functions[1],
                    functions[2]));
                state = TwitchState.VOTING;
            }
            

            //if (Input.GetKeyDown(KeyCode.P)) {
            //    PrintVotes(voteDict);
            //}
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

    public void Set_Timer(float timer) {
        votingTimer = timer;
    }

    void PrintVotes(Dictionary<string, int> dict) {

        foreach (KeyValuePair<string, int> entry in dict) {
            Debug.Log(entry.Key + entry.Value);
        }

    }

    string[] GetTwitchFunctionNames() {

        string[] names = new string[3];

        for (int i = 0; i < names.Length; i++) {

            foreach(KeyValuePair<string, TwitchFunctionsEnum> pair in twitchFunctions.dict) {
                if(pair.Value == (TwitchFunctionsEnum)i) {
                    names[i] = pair.Key;
                    Debug.Log(names[i]);
                }
            }
        }

        return names;

    }

    void GetResults() {

        int winningVote = TallyVotes();
        MethodInfo method;
        switch (winningVote) {

            case 1:
                method = twitchFunctions.GetType().GetMethod(functions[0]);
                method.Invoke(twitchFunctions, new object[] {  });
                //twitchFunctions.Invoke(functions[0], 5f);
                break;
            case 2:
                method = twitchFunctions.GetType().GetMethod(functions[1]);
                method.Invoke(twitchFunctions, new object[] {  });
                break;
            case 3:
                method = twitchFunctions.GetType().GetMethod(functions[0]);
                method.Invoke(twitchFunctions, new object[] {  });
                break;
            default:
                Debug.Log("no vote has won");
                break;
        }
    }

    int TallyVotes() {

        int total1 = 0, total2 = 0, total3 = 0;

        foreach(KeyValuePair<string, int> valuePair in voteDict) {

            switch (valuePair.Value) {

                case 1:
                    total1++;
                    break;
                case 2:
                    total2++;
                    break;
                case 3:
                    total3++;
                    break;
                default:
                    continue;
            }

        }

        if (total1 > total2 && total1 > total3) {
            return 1;
        }
        else if (total2 > total1 && total2 > total3) {
            return 2;
        }
        else if (total3 > total1 && total3 > total2) {
            return 3;
        }
        else if (total1 == total2 && total1 == total3 && total2 == total3) {
            return 0;
        }
        else if (total1 == total2) {
            return Random.Range(1, 3);
        }
        else if (total1 == total3) {

            int value = Random.Range(1, 3);

            if (value == 1)
                return 1;
            else
                return 3;

        }
        else if (total2 == total3) {
            return Random.Range(2, 4);
        }

        return 0;

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

    public void SendMessageToTwitch(string message, int timer) {

        writer.WriteLine(sendMessagePrefix + message + " " + timer + " seconds remaining");
        writer.Flush();
    }

    public void SendMessageToTwitch(string message, string command) {

        writer.WriteLine(sendMessagePrefix + command + ' ' + message);
        writer.Flush();
    }
}
