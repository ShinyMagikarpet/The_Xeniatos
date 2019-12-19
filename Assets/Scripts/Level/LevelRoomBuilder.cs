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
            teleporters[i].ID = i;
            teleporters[i].linkedID = teleporters[i].linkedTeleporter.ID;
            teleporterObjects[i] = teleporters[i].gameObject;
            if (teleporters[i].linkedTeleporter) {
                teleporters[i].linkedTeleporter.linkedTeleporter = teleporters[i];
            }
            if(teleporters[i].linkedTeleporter == null) {
                Debug.Log("teleporter " + i + " doesn't have a linked teleporter");
            }
        }
        
    }
}
