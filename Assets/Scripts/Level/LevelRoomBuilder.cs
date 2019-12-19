using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LevelRoomBuilder : MonoBehaviour
{
    public GameObject room;
    public GameObject[] teleporterObjects;

    // Update is called once per frame
    void Update(){
        Teleporter[] teleporters = GetComponentsInChildren<Teleporter>();
        Debug.Log(teleporters.Length);
        for(int i = 0; i < teleporters.Length; i++) {
            teleporterObjects[i] = teleporters[i].gameObject;
        }
        
    }
}
