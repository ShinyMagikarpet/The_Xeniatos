using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grow_Height : MonoBehaviour
{
    float growRate = 0.23f;
    float maxHeight = 8f;
    [SerializeField]Vector3 originalPos;
    [SerializeField]Vector3 originalScale;
    [SerializeField] Weapon mWeapon;

    private void Start() {
        originalPos = transform.localPosition;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) {
            Grow_Object(growRate);
        }

        if (Input.GetButtonUp("Fire1")) {
            transform.localPosition = originalPos;
            transform.localScale = originalScale;
        }
    }

    void Grow_Object(float rate) {

        if (transform.localScale.y < maxHeight) {
            transform.localScale += new Vector3(0, rate, 0);
            transform.Translate(0, rate / 2, 0);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && other.GetComponent<Player>() != mWeapon.mOwner) {
            other.GetComponent<PhotonView>().RPC("Take_Damage", RpcTarget.All, mWeapon.mDamage);
        }
    }
}
