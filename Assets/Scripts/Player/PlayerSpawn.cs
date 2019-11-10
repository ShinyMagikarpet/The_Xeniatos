using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour{

    public byte teamNum;

    public void Spawn_Player(Player player){

        player.transform.position = this.transform.position;
    }
}
