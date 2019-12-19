using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PillowWeapon : Weapon
{
    public GameObject mPivot;
    public bool mIsSwinging;
    public PillowWeapon() {
        mName = "Pillow";
        mROF = 0.5f;
        mDamage = 50f;
        mSwingSpeed = 10f;
        mIsMelee = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.CompareTag("Player")) {
            Debug.Log("Hit the player");
            if (PhotonNetwork.IsConnected)
                other.GetComponent<PhotonView>().RPC("Take_Damage", RpcTarget.AllBuffered, mDamage);
            else
               other.GetComponent<Player>().Take_Damage(mDamage);
        }
    }
}
