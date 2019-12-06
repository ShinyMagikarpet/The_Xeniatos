using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPad : MonoBehaviour{

    float force = 10.0f;
    bool isPushing = false;
    CharacterController cc;

    private void Update() {

        if (isPushing && cc) {
            cc.Move(transform.forward * force * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Weeb")) {
            Debug.Log("Moo");
            cc = other.GetComponent<CharacterController>();
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed = 0f;
            isPushing = true;
            StartCoroutine(ContinuousPush(3f, controller));
        }
    }

    IEnumerator ContinuousPush(float time, PlayerController controller) {

        yield return new WaitForSeconds(time);
        controller.speed = 8f;
        isPushing = false;
    }
}
