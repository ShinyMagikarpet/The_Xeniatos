using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ParticleProjectile : MonoBehaviour
{
    public string mName;
    public float mDamage;
    public ParticleSystem mParticles;
    private Player mOwner;
    private PhotonView mPV;


    private void Start() {
        mParticles = GetComponent<ParticleSystem>();
        mOwner = GetComponentInParent<Player>();
        mPV = mOwner.gameObject.GetComponent<PhotonView>();
        

        //mParticles.Play();
    }

    [PunRPC]
    public void Play_Particles_Weapon() {

        //Debug.Log("Play animation");
        mParticles.Play();

    }

    [PunRPC]
    public void Stop_Particles_Weapon()
    {

        mParticles.Stop();

    }

    private void OnParticleCollision(GameObject other) {

        if (other.CompareTag("Player") && other != mOwner) {
            other.GetComponent<PhotonView>().RPC("Take_Damage", RpcTarget.All, mDamage);
        }
    }

    public Player Get_Owner()
    {
        return mOwner;
    }

    public PhotonView Get_PhotonView()
    {
        return mPV;
    }
}
