using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour{

    public float jumpValue = 20f;
    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player") || other.CompareTag("Weeb")) {

            Debug.Log("Player has stepped on jump pad");

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.SetVerticalVelocity(jumpValue);
            
        }
    }
}
