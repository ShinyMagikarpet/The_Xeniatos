using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {
    
    public void ChangeScenes(int sceneIndex) {

        SceneManager.LoadScene(sceneIndex);
    }

    public void GotoMainMenu() {

        SceneManager.LoadScene(0);
    }
}
