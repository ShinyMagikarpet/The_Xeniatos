using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WriteToFile : MonoBehaviour
{

    public InputField inputField;
    [SerializeField] private TwitchChatManager twitchChat;

    private void Awake() {
        twitchChat = FindObjectOfType<TwitchChatManager>();
    }

    public void Set_Twitch_Username() {

        if (inputField.text.Length > 0) {
            Debug.Log("Writing out " + "\"" + inputField.text + "\" to file.");

            //string message = HashString(inputField.text);

            twitchChat.username = inputField.text;

            inputField.text = "";
        }
        else
            Debug.Log("Write something in the field dummy");
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
