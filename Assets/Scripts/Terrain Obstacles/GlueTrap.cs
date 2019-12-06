using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueTrap : MonoBehaviour{

    float playerBaseSpeed = 6.0f;
    float weebBaseSpeed = 8.0f;
    float weebBaseJumpSpeed = 5.0f;
    float weebBaseSprint = 0f;
    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Weeb")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed *= 0.33f;
            controller.jumpSpeed *= 0.75f;
            weebBaseSprint = controller.dashSpeed;
            controller.dashSpeed = 1f;
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.CompareTag("Weeb")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed = weebBaseSpeed;
            controller.jumpSpeed = weebBaseJumpSpeed;
            controller.dashSpeed = weebBaseSprint;
        }
    }
}
