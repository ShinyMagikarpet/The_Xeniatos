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
        projectile.transform.rotation = transform.rotation;
        projectile.transform.forward = transform.forward;
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * mSpeed, ForceMode.Force);
        
        StartCoroutine(Despawn(mTTL, projectile));
        //Debug.Log("projectile active is " + projectile.gameObject.activeSelf);


    }

    IEnumerator Despawn(float time, Projectile projectile) {


        yield return new WaitForSeconds(time);
        projectile.gameObject.transform.position = new Vector3(0, 0, 0);
        projectile.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        projectile.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        projectile.gameObject.SetActive(false);
    }



}
