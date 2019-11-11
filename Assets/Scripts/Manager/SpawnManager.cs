using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour{

    public static SpawnManager Instance;
    [SerializeField]private PlayerSpawn[] spawnList;

    // Start is called before the first frame update
    void Awake(){

        if (!Instance){
            Instance = this;

            spawnList = GetComponentsInChildren<PlayerSpawn>();
        }
    }

    public void Spawn_Player(int teamNum, Player player) {

        Debug.Log("spawn list size is " + spawnList.Length);

        foreach(PlayerSpawn playerSpawn in spawnList) {
            if(playerSpawn.teamNum == teamNum) {
                Debug.Log("Successfully spawned player's position");
                playerSpawn.Spawn_Player(player);
            }
        }
    }

    public PlayerSpawn Get_Spawn_From_Team(int teamNum) {

        foreach (PlayerSpawn playerSpawn in spawnList) {
            if (playerSpawn.teamNum == teamNum) {
                return playerSpawn;
            }
        }

        Debug.Log("No appropriate spawn found");
        return null;
    }

}
