using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour{


    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player") || other.CompareTag("Weeb")) {

            Debug.Log("Player has stepped on jump pad");

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.SetVerticalVelocity(15f);
            
        }
    }
}
