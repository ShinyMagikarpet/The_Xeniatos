using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour{
    bool isCollected = false;
    bool isDeposited = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isCollected = true;
            gameObject.SetActive(false);
        }
    }

    public bool GetCollectedStatus() {
        return isCollected;
    }
}
