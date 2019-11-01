using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Projectile : MonoBehaviourPunCallbacks
{

    public float mSpeed;
    public float mTTL;
    public string mName;
    public float mDamage;
    public GameObject mOwner;

    public void Shoot_Projectile(Projectile projectile, Vector3 camForward, Vector3 pos) {

        
        projectile.transform.position = pos;
        projectile.transform.up = transform.forward;
        projectile.GetComponent<Rigidbody>().AddForce(camForward * mSpeed, ForceMode.Force);
        
        StartCoroutine(Despawn_Time(mTTL, projectile));
        //Debug.Log("projectile active is " + projectile.gameObject.activeSelf);

    }

    IEnumerator Despawn_Time(float time, Projectile projectile) {


        yield return new WaitForSeconds(time);
        Despawn_Projectile(projectile);
    }

    protected void Despawn_Projectile(Projectile projectile) {
        projectile.gameObject.transform.position = new Vector3(0, 0, 0);
        projectile.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        projectile.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        projectile.gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player") && collision.gameObject != mOwner) {
            if (PhotonNetwork.IsConnected)
                collision.collider.GetComponent<PhotonView>().RPC("Take_Damage", RpcTarget.All, mDamage);
            else
                collision.collider.GetComponent<Player>().Take_Damage(mDamage);
            Despawn_Projectile(this);
        }
    }


}
