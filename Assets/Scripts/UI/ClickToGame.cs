using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickToGame : MonoBehaviour
{

    public void Find_Game() {
        Debug.Log("Switching scenes");
        SceneManager.LoadScene("Debug");
    }
    
}
