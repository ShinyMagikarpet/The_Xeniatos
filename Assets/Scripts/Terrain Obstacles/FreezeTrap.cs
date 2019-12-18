using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour{

    private float freezeTime = 10.0f;
    private bool isFreezingPlayer = false;
    private Player weeb;
    private void OnTriggerEnter(Collider other) {

        if (isFreezingPlayer) return;

        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed = 0.0f;
            controller.jumpSpeed = 0.0f;
            Player[] players = FindObjectsOfType<Player>();
            foreach(Player player in players) {
                if (player.IsWeeb) {
                    weeb = player;
                    break;
                }
            }
            
            StartCoroutine(FreezePlayer(freezeTime, controller));
        }
    }

    IEnumerator FreezePlayer(float timer, PlayerController playerController) {
        yield return new WaitForSeconds(timer);
        playerController.speed = playerController.GetPlayerStartSpeed();
        playerController.jumpSpeed = playerController.GetPlayerStartJumpSpeed();
    }

}
