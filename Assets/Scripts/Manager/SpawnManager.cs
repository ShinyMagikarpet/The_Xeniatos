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

        foreach(PlayerSpawn playerSpawn in spawnList) {
            if(playerSpawn.teamNum == teamNum) {
                playerSpawn.Spawn_Player(player);
            }
        }
    }

}
