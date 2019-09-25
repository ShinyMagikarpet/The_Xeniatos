using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool mIsDead = false;
    public Weapon mPlayerWeapon;

    void Start()
    {
        mPlayerWeapon = new HandGun();
        mPlayerWeapon.mCam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            mPlayerWeapon.Fire_Weapon();
        }
    }
}
