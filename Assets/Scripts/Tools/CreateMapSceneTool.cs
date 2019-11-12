using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateMapSceneTool : MonoBehaviour {

    [MenuItem("Map/Create Multiplayer Map")]
    static void CreateMap() {
        Debug.Log("Creating Scene!");
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            CreateScene();
    }


    static void CreateScene() {

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject managers = new GameObject(name: "Managers"); // Create new Game object

        //Managers
        Instantiate(Resources.Load("_NetworkManager"), managers.transform);
        Instantiate(Resources.Load("_ObjectManager"), managers.transform);
        Instantiate(Resources.Load("_SpawnManager"), managers.transform);
        Instantiate(Resources.Load("_GameManager"), managers.transform);
        Instantiate(Resources.Load("_PlayerManager"), managers.transform);

        //UI Elements
        GameObject canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.layer = 5; //UI is layer 5
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }
}
