using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{

    public Grow_Height grow;
    
    public Flamethrower() {

        mName = "Rainbow Flamethrower";
        mDamage = 0.5f;
        mROF = 0.1f;
        mRange = 3.8f;
        mReloadSpeed = 1.8f;
        mAmmoLoaded = 200;
        mMaxAmmoLoaded = 200;
        mAmmoHeld = 400;
        mMaxAmmo = 400;
        mFire_Type = Fire_Type.Beam;
        mIsBeam = true;
        mIsParticle = false;
        mIsProjectile = false;
    }

    private void Awake() {
        mParticleSystem = GetComponentInChildren<ParticleSystem>();
        grow = GetComponentInChildren<Grow_Height>();
    }

    private void Update() {
        
    }
}
