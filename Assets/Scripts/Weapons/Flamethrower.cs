using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{

    public Grow_Height grow;
    public Renderer weaponBase;
    public Renderer weaponBolt;
    public Renderer weaponMag;
    //private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private float value = 100.0f;

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
        _propBlock = new MaterialPropertyBlock();
    }

    private void Update() {
        Change_Material();
    }

    private void Change_Material() {
        //value = (float)mAmmoLoaded / (float)mMaxAmmoLoaded;
        value = (float)mAmmoLoaded / (float)mMaxAmmoLoaded;
        Debug.Log(value);

        weaponBase.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_Transition", value);
        weaponBase.SetPropertyBlock(_propBlock);

        weaponBolt.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_Transition", value);
        weaponBolt.SetPropertyBlock(_propBlock);

        weaponMag.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_Transition", value);
        weaponMag.SetPropertyBlock(_propBlock);
    }
}
