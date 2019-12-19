using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

    public Teleporter linkedTeleporter;
    private float cooldown = 0.0f;
    private float cooldownRate = 5.0f;
    private bool isOnCooldown = false;
    private float hackedCooldownRate = 60.0f;
    private bool isHacked = false;
    public int ID;
    public int linkedID = -1;

    public void OnTriggerStay(Collider other) {

        if (isOnCooldown) return;

        if (other.CompareTag("Weeb") || other.CompareTag("Player")) {
            linkedTeleporter.isOnCooldown = true;
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = linkedTeleporter.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
            isOnCooldown = true;
            StartCoroutine(TeleportCooldown(cooldownRate));

        }
    }

    IEnumerator TeleportCooldown(float timer) {
        yield return new WaitForSeconds(timer);
        isOnCooldown = false;
        linkedTeleporter.isOnCooldown = false;
    }

    public int Get_Linked_ID() {
        if(linkedTeleporter)
            return linkedTeleporter.linkedID;
        return -1;
    }
}
