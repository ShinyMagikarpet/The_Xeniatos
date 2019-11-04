using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNet : MonoBehaviour{

    private void Update() {
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.gameObject.SetActive(false);
            other.transform.position = Vector3.zero;
            other.gameObject.SetActive(true);
        }
    }
}
