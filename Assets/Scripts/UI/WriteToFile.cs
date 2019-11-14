using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WriteToFile : MonoBehaviour
{
    
    public InputField username, password, channelName;
    DateTime lastMessage;

    public void Set_Twitch_Username() {
        
        if (username.text.Length > 0) {

            //string message = HashString(inputField.text);

            TwitchChatManager.Instance.username = username.text;

            username.text = "";
        }
        else
            Debug.Log("Write something in the field dummy");
    }

    public void Set_Twitch_Password() {

        if (password.text.Length > 0) {

            //string message = HashString(inputField.text);

            TwitchChatManager.Instance.password = password.text;

            password.text = "";
        }
        else
            Debug.Log("Write something in the field dummy");
    }

    public void Set_Twitch_ChannelName() {
        
        if (channelName.text.Length > 0) {

            //string message = HashString(inputField.text);

            TwitchChatManager.Instance.channelName = channelName.text;

            channelName.text = "";
        }
        else
            Debug.Log("Write something in the field dummy");
    }

    public void ConnectToTwitch() {

        if(TwitchChatManager.Instance != null) {
            Set_Twitch_Username();
            Set_Twitch_Password();
            Set_Twitch_ChannelName();
            TwitchChatManager.Instance.Connect();
        }
    }

    public void MessageTwitch() {
        if (DateTime.Now - lastMessage > TimeSpan.FromSeconds(2)) {
            TwitchChatManager.Instance.SendMessageToTwitch("!vote 1");
            lastMessage = DateTime.Now;
        }
    }

    /* File stuff
     
    #region File Usage
    private void CreateFile(string content) {

        //File path
        string path = Application.dataPath + "/Resources/Log.txt";



        //Clear and write to file
        File.WriteAllText(path, content);

    }

    public void ReadFromFile() {

        string path = Application.dataPath + "/Resources/Log.txt";

        if (!File.Exists(path)) {
            Debug.LogError("This file doesn't exist in context to the path");
        }
        else {
            Debug.Log(File.ReadAllText(path));
        }
    }
    #endregion

    #region File Hashing
    private string HashString(string message) {
        StringBuilder newMessage = new StringBuilder();

        byte[] hashBytes = new byte[0];
        HashAlgorithm hashCode;

        hashCode = SHA512.Create();
        hashBytes = hashCode.ComputeHash(Encoding.UTF8.GetBytes(message));

        foreach(byte _byte in hashBytes) {
            newMessage.Append(_byte.ToString("X2"));
        }

        return newMessage.ToString();
    }
    #endregion
    */
}
