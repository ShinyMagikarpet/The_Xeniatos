using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Projectile : Projectile
{
    bool isRising = false;
    public Bubble_Projectile() {
        mName = "Bubble";
        mSpeed = 100.0f;
        mTTL = 1.0f;
        mDamage = 10.0f;
    }

    private void Update() {

        if (!isRising) {
            Arise_Bubble(2f);
        }
        else {
            GetComponent<Rigidbody>().AddForce(0, 10, 0, ForceMode.Acceleration);
            Debug.Log("Arise bubble");
        }
    }

    IEnumerator Arise_Bubble(float time) {
        Debug.Log("Waiting");
        Debug.Log(isRising);
        yield return new WaitForSeconds(time);

        isRising = true;
        Debug.Log(isRising);
    }
}
