using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleProjectile : MonoBehaviour
{
    public string mName;
    public float mDamage;
    public ParticleSystem mParticles;
    public GameObject mOwner;

    private void Start() {
        mParticles = GetComponent<ParticleSystem>();
        //mParticles.Play();
    }

    public void Fire_Particles(ParticleSystem particles) {

        mParticles.Play();

    }

    private void OnParticleCollision(GameObject other) {

        if (other.CompareTag("Player") && other != mOwner) {
            other.GetComponent<Player>().mCurrentHealth -= mDamage;
        }
    }

}
