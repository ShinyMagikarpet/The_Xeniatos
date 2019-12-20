using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{

    public Teleporter linkedTeleporter;
    private float cooldownRate = 15.0f;
    private bool isOnCooldownWeeb = false;
    private bool isOnCooldownPlayer = false;
    public int ID;
    public int linkedID = -1;

    public void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Weeb") && !isOnCooldownWeeb) {
            linkedTeleporter.isOnCooldownWeeb = true;
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = linkedTeleporter.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
            isOnCooldownWeeb = true;
            StartCoroutine(TeleportCooldownWeeb(cooldownRate * 0.1f));
        }

        if (other.CompareTag("Player") && !isOnCooldownPlayer) {
            linkedTeleporter.isOnCooldownPlayer = true;
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = linkedTeleporter.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
            isOnCooldownPlayer = true;
            StartCoroutine(TeleportCooldownPlayer(cooldownRate));

        }
    }

    IEnumerator TeleportCooldownWeeb(float timer) {
        yield return new WaitForSeconds(timer);
        isOnCooldownWeeb = false;
        linkedTeleporter.isOnCooldownWeeb = false;
    }

    IEnumerator TeleportCooldownPlayer(float timer) {
        yield return new WaitForSeconds(timer);
        isOnCooldownWeeb = false;
        linkedTeleporter.isOnCooldownWeeb = false;
    }

    public int Get_Linked_ID() {
        if(linkedTeleporter)
            return linkedTeleporter.linkedID;
        return -1;
    }
}
