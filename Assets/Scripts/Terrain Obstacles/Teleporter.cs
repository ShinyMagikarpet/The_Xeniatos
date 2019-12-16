using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

    public void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.CompareTag("Weeb")) {
            Debug.Log("You stepped on me collider");
        }
        else {
            Debug.Log("Hello");
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Weeb")) {
            Debug.Log("You stepped on me trigger");
        }
    }
}
