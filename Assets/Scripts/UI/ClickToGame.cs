using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickToGame : MonoBehaviour
{

    public InputField input;

    public void Find_Game() {

        if (!Valid_Input(input.text)) {
            Debug.Log("Not valid input");
            return;
        }

        Debug.Log("Switching scenes");
        SceneManager.LoadScene("Debug");
    }

    bool Valid_Input(string inputText) {
        
        if (inputText.Length > 10 || string.IsNullOrWhiteSpace(inputText)) {
            input.text = "";
            return false;
        }

        return true;
    }
    
}
