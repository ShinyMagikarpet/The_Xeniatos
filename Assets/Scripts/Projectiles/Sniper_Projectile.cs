using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Projectile : Projectile{

    public Sniper_Projectile(){
        mName = "Sniper Bullet";
        mSpeed = 1000.0f;
        mTTL = 1.7f;
        mDamage = 50.0f;
    }

    void Despawn_Sniper_Bullet() {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StopAllCoroutines();
        StartCoroutine(Let_Trail_Finish(0.5f));
    }

    IEnumerator Let_Trail_Finish(float time) {

        yield return new WaitForSeconds(time);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        Despawn_Projectile(this);

    }
    protected override void OnCollisionEnter(Collision m_collision) {

        base.OnCollisionEnter(m_collision);

        if (m_collision.collider.gameObject != mOwner) {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Despawn_Sniper_Bullet();
        }
        else if(m_collision.collider.tag == "Player" && m_collision.collider.gameObject != mOwner) {
            base.OnCollisionEnter(m_collision);
        }
    }

}

