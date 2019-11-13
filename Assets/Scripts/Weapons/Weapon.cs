using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public abstract class Weapon : MonoBehaviourPunCallbacks {

    public enum Fire_Type{
        single,
        Semi_Auto,
        fully_Auto,
        Beam,
        Particle
    }

    public string mName;                    /*<mName Name of weapon*/
    public float mDamage;                   /*<Amount of damage*/
    public float mROF;                      /*<Rate of fire*/
    public float mRange;                    /*<Max range*/
    public float mReloadSpeed;              /*<Reload speed*/
    public float mSpread;                   /*<Weapon spread*/
    public int mAmmoLoaded;                 /*<Current loaded ammo*/
    public int mMaxAmmoLoaded;              /*<Max Ammo allowed to be loaded*/
    public int mMaxAmmo;                    /*<Maximum carried ammo*/
    public int mAmmoHeld;                   /*<Current ammo being held*/
    public Fire_Type mFire_Type;            /*<Fire mode for weapon*/
    public bool mIsProjectile;              /*<Does weapon shoot projectiles*/
    public bool mIsParticle;                /*<Does the weapon shoot particles*/
    public bool mIsBeam;                    /*<Is the weapon a Beam Type*/
    public ParticleSystem mParticleSystem;  /*<Particle system that the weapon will use for visual effects*/
    public ParticleProjectile mParticleProjectile;  /*<Particle system that the weapon will be firing*/
    public Projectile bullet;               /*<What projectile the weapon will be firing*/
    public Player mOwner;                   /*<The owner of the weapon*/

    private float mTimeToNextFire;
    private bool mIsReloading = false;
    private ObjectPool objectPool;

    public Camera mCam;

    private PhotonView PV;


    private void Start() {
        Weapon_Setup();

    }

    public void Weapon_Setup() {
        
        objectPool = ObjectPool.Instance;
        mOwner = GetComponentInParent<Player>();
        mOwner.mPlayerWeapon = this;
        PV = mOwner.GetComponent<PhotonView>();
        mCam = GetComponentInParent<Camera>();
        mAmmoHeld = mMaxAmmo;
        mAmmoLoaded = mMaxAmmoLoaded;
        
        
    }

    public void Fire_Weapon() {

        if (mIsProjectile) {
            if(PhotonNetwork.IsConnected)
                photonView.RPC("Fire_Projectile", RpcTarget.AllBuffered, mCam.transform.forward, mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.5f)));
            else
                Fire_Projectile(mCam.transform.forward, mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.5f)));
        } 
        else if (mIsParticle) {
            Fire_Particles();
        }
        else if (mIsBeam) {
            Fire_Beam();
        }
        else {
            Fire_Hitscan();
        }

    }
    
    public void Fire_Hitscan() {


        if (!Can_Fire())
            return;

        if (Time.time > mTimeToNextFire){

            mOwner.state = Player.PlayerState.Shooting;
            mTimeToNextFire = Time.time + mROF;

            if(mParticleSystem != null) {
                mParticleSystem.Play();
            }

            Vector3 rayOrigin = mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;


            Debug.DrawRay(rayOrigin, mCam.transform.forward * mRange, Color.cyan);

            if (Physics.Raycast(rayOrigin, mCam.transform.forward, out hit, mRange)){
                if (hit.collider.CompareTag("Player") && hit.collider.gameObject != mOwner.gameObject) {
                    
                    Debug.Log("You hit " + hit.collider.name + "!");
                    if (PhotonNetwork.IsConnected)
                        hit.collider.GetComponent<PhotonView>().RPC("Take_Damage", RpcTarget.AllBuffered, mDamage);
                    else
                        hit.collider.GetComponent<Player>().Take_Damage(mDamage);
                }
                    

                if (hit.collider.CompareTag("Environment")) {
                    hit.collider.GetComponent<Rigidbody>().AddForce(mCam.transform.forward * 100.0f);
                    Debug.Log("You hit an environmental piece named " + hit.collider.name);
                }
            }

            mAmmoLoaded--;
        }

    }

    [PunRPC]
    public void Fire_Projectile(Vector3 camForward, Vector3 position) {


        if (!Can_Fire())
            return;

        if (Time.time > mTimeToNextFire) {

            mOwner.state = Player.PlayerState.Shooting;
            mTimeToNextFire = Time.time + mROF;
            Projectile projectile = objectPool.SpawnFromPool(bullet.mName).GetComponent<Projectile>();
            if(projectile == null) {
                projectile = objectPool.SpawnFromPool(bullet.mName).GetComponentInChildren<Projectile>();
            }
            projectile.mOwner = mOwner.gameObject;
            //mCam.transform, mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.5f))
            projectile.Shoot_Projectile(projectile, camForward, position);

            mAmmoLoaded--;
        }
        

    }

    public void Fire_Particles() {


        if (!Can_Fire())
            return;

        if (Time.time > mTimeToNextFire) {
            Player owner = mParticleProjectile.Get_Owner();
            owner = mOwner;
            mOwner.state = Player.PlayerState.Shooting;
            mTimeToNextFire = Time.time + mROF;

            Debug.Log(mParticleProjectile.mParticles.name);
            //mParticleProjectile.Fire_Particles();

            if (!mParticleProjectile.mParticles.isEmitting) {
                if (PhotonNetwork.IsConnected)
                    mOwner.photonView.RPC("Play_Particle_Projectile", RpcTarget.AllBuffered);
                else
                    mOwner.Play_Particle_Projectile();
            }
                

            mAmmoLoaded--;
        }


    }

    void Fire_Beam() {

        if (!Can_Fire())
            return;

        if (Time.time > mTimeToNextFire) {

            mTimeToNextFire = Time.time + mROF;


            if (!mParticleSystem.isEmitting) {
                if (PhotonNetwork.IsConnected)
                    mOwner.photonView.RPC("Play_Particle_System", RpcTarget.AllBuffered);
                else
                    mOwner.Play_Particle_System();
            }
                

            mAmmoLoaded--;
        }

    }

    bool Can_Fire() {

        if (mAmmoLoaded <= 0) {
            mAmmoLoaded = 0;
            if (mFire_Type == Fire_Type.Particle) {
                if (mParticleProjectile.mParticles.isEmitting) {
                    if (PhotonNetwork.IsConnected)
                        mOwner.photonView.RPC("Stop_Particle_Projectile", RpcTarget.AllBuffered);
                    else
                        mOwner.Stop_Particle_Projectile();
                }
            } 
            else if(mFire_Type == Fire_Type.Beam){
                if (mParticleSystem.isEmitting) {
                    if (PhotonNetwork.IsConnected)
                        mOwner.photonView.RPC("Stop_Particle_System", RpcTarget.AllBuffered);
                    else
                        mOwner.Stop_Particle_System();
                }
            }
            else {
                if(mParticleSystem != null) {
                    mParticleSystem.Stop();
                }
            }
            return false;
        }

        if (mIsReloading) {
            if (mFire_Type == Fire_Type.Particle) {
                if (mParticleProjectile.mParticles.isEmitting) {
                    if (PhotonNetwork.IsConnected)
                        mOwner.photonView.RPC("Stop_Particle_Projectile", RpcTarget.AllBuffered);
                    else
                        mOwner.Stop_Particle_Projectile();
                }
            }
            else if(mFire_Type == Fire_Type.Beam) {
                if (mParticleSystem.isEmitting) {
                    if (PhotonNetwork.IsConnected)
                        mOwner.photonView.RPC("Stop_Particle_System", RpcTarget.AllBuffered);
                    else
                        mOwner.Stop_Particle_System();
                }
            }
            return false;
        }

        return true;
    }

    public void Reload_Weapon() {

        if (mIsReloading || mAmmoLoaded == mMaxAmmoLoaded || mAmmoHeld == 0)
            return;


        StartCoroutine(Reloading(mReloadSpeed));
        

    }

    IEnumerator Reloading(float time) {

        mIsReloading = true;
        mOwner.state = Player.PlayerState.Reloading;

        yield return new WaitForSeconds(time);

        if (mAmmoHeld >= mMaxAmmoLoaded && mAmmoLoaded == 0) {
            mAmmoHeld -= mMaxAmmoLoaded;
            mAmmoLoaded = mMaxAmmoLoaded;
        } else if (mAmmoHeld >= mMaxAmmoLoaded && mAmmoLoaded > 0) {
            int difference = mMaxAmmoLoaded - mAmmoLoaded;
            mAmmoLoaded += difference;
            mAmmoHeld -= difference;
        } else if (mAmmoHeld < mMaxAmmoLoaded && mAmmoLoaded > 0) {

            int difference;
            int ammoNeeded = mMaxAmmoLoaded - mAmmoLoaded;
            Debug.Log("Ammo needed = " + ammoNeeded);
            if (ammoNeeded < mAmmoHeld)
                difference = mAmmoHeld - ammoNeeded;
            else
                difference = mAmmoHeld;
            

            mAmmoLoaded += difference;
            mAmmoHeld -= difference;
        } else {
            mAmmoLoaded = mAmmoHeld;
            mAmmoHeld = 0;
        }

        mOwner.state = Player.PlayerState.Idle;
        mIsReloading = false;
    }
}


