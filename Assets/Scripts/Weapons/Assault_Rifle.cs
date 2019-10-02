using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assault_Rifle : Weapon
{
    public Assault_Rifle() {
        mName = "Assault_Rifle";
        mMaxAmmo = 150;
        mAmmoHeld = mMaxAmmo;
        mMaxAmmoLoaded = 30;
        mAmmoLoaded = mMaxAmmoLoaded;
        mDamage = 8.5f;
        mRange = 250.0f;
        mReloadSpeed = 2.5f;
        mROF = 0.10f;
        mIsProjectile = false;
        mFire_Type = Fire_Type.fully_Auto;
    }
}
