using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon{
    public Sniper(){

        mName = "Sniper";
        mMaxAmmo = 40;
        mAmmoHeld = mMaxAmmo;
        mMaxAmmoLoaded = 10;
        mAmmoLoaded = mMaxAmmoLoaded;
        mDamage = 50f;
        mRange = 500.0f;
        mReloadSpeed = 3.5f;
        mROF = 0.8f;
        mIsProjectile = false;
        mFire_Type = Fire_Type.single;

    }
}
