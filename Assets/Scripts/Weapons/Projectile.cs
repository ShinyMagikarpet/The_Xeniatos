using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public float mSpeed;
    public float mTTL;
    public string mName;

    public void Shoot_Projectile(Projectile projectile, Transform transform, Vector3 pos) {

        projectile.transform.position = pos;
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * mSpeed);
        
        StartCoroutine(Despawn(mTTL, projectile));
        //Debug.Log("projectile active is " + projectile.gameObject.activeSelf);


    }

    IEnumerator Despawn(float time, Projectile projectile) {


        yield return new WaitForSeconds(time);

        projectile.gameObject.SetActive(false);
    }



}
