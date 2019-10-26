using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{

    

    public enum PlayerState {
        Idle,
        Walking,
        Sprinting,
        Jumping,
        Shooting,
        Reloading,
        Dead,
        InMenu,
        Collecting,
        Crafting
    };

    [HideInInspector] public bool mIsDead = false;                    
    public float mMaxHealth = 100f;
    public float mCurrentHealth = 100f;
    public Weapon mPlayerWeapon;
    public Weapon mPlayerSubWeapon;
    public PlayerState state;
    public GameObject mPlayerUIPrefab;
    public GameObject mArmsMesh;
    public GameObject mFullBodyMesh;
    public GameObject mRagdoll;
    public Dictionary<string, int> mResourceDict;

    public static GameObject LocalPlayerInstance;
    [SerializeField]
    private ResourceNode _targetNode;
    private GameObject _lastWorkBench;
    private float mCollectRate = 1.3f;
    private float mTimeToNextCollect;
    private bool mIsCollecting = false;
    private bool mIsCrafting = false;
    private MaterialPropertyBlock _propblock;

    [SerializeField] private Camera mCam;
    [SerializeField] private Camera mMinimapCam;

    void Start(){
        
        //Disable/Enable whatever you want the local client to see for remote users here
        if (!photonView.IsMine && PhotonNetwork.IsConnected == true) {
            mCam.enabled = false;
            mCam.GetComponent<AudioListener>().enabled = false;
            mMinimapCam.enabled = false;
            mPlayerWeapon = mCam.gameObject.GetComponentInChildren<Weapon>();
            mArmsMesh.SetActive(false);
            mFullBodyMesh.SetActive(true);
            GetComponent<PlayerController>().enabled = false;
            this.name = PhotonNetwork.NickName;
            return;
        }
        mPlayerWeapon.mOwner = this;
        state = PlayerState.Idle;
        //mFullBodyMesh = Get_Player_Mesh();
        this.name = PhotonNetwork.NickName;
        mResourceDict = new Dictionary<string, int>();
        mResourceDict.Add("Iron", 0);
        mResourceDict.Add("Stone", 0);
        mResourceDict.Add("Wood", 0);

        mPlayerUIPrefab = Instantiate(mPlayerUIPrefab);
        mPlayerUIPrefab.GetComponent<PlayerUI>().SetTarget(this);

        _propblock = new MaterialPropertyBlock();
        
    }

    // Update is called once per frame
    void Update(){
        
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if(mIsCollecting) {
            Player_Gather_Resource();
        }

        //TODO: Needs better 3rd person camera to better show rigidbody
        if(mCurrentHealth <= 0 && !mIsDead) {
            //Player_Die();
            //return;
            Debug.Log("Want to play around with this later");
        }
        

        Player_Inputs();

    }


    void Player_Inputs() {

        //Debug.DrawRay(mPlayerWeapon.mCam.transform.position, mPlayerWeapon.mCam.transform.TransformDirection(Vector3.forward) * 3, Color.cyan);

        if(Player_Near_Resource() && Input.GetButtonDown("Interact"))
        {
            mIsCollecting = true;
        }

        if (Player_Near_Workbench() && Input.GetButtonDown("Interact")) {
            state = PlayerState.Crafting;
            mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Craft_Menu();
            mIsCrafting = true;
        }

        //Menu
        if (Input.GetButtonDown("Cancel")) {

            if(state == PlayerState.Crafting) {
                state = PlayerState.Idle;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Craft_Menu();
            }
            else if (state != PlayerState.InMenu) {
                Debug.Log("Entering menu");
                state = PlayerState.InMenu;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Pause_Menu();
            } else {
                Debug.Log("Leaving menu");
                state = PlayerState.Idle;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Pause_Menu();
            }

        }

        if(state == PlayerState.Crafting) {
            return;
        }

        //Single fire
        if (Input.GetButtonDown("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.single && state != PlayerState.InMenu) {
            mPlayerWeapon.Fire_Weapon();
        }

        //Automatic fire
        else if (Input.GetButton("Fire1") && (mPlayerWeapon.mFire_Type == Weapon.Fire_Type.fully_Auto || mPlayerWeapon.mFire_Type == Weapon.Fire_Type.Beam || mPlayerWeapon.mFire_Type == Weapon.Fire_Type.Particle) && 
            state != PlayerState.InMenu) {

                mPlayerWeapon.Fire_Weapon();
             }

        //Reload
        if (Input.GetButtonDown("Reload") || mPlayerWeapon.mAmmoLoaded == 0 && state != PlayerState.InMenu) {
            mPlayerWeapon.Reload_Weapon();
        }

        if(mPlayerWeapon.mIsParticle && mPlayerWeapon.mParticleProjectile.mParticles.isEmitting && !Input.GetButton("Fire1")) {
            //mPlayerWeapon.mParticleProjectile.mParticles.Stop();
            photonView.RPC("Stop_Particle_Projectile", RpcTarget.All);
        }

        if(mPlayerWeapon.mIsBeam && mPlayerWeapon.mParticleSystem.isEmitting && !Input.GetButton("Fire1")) {
            photonView.RPC("Stop_Particle_System", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            //mCurrentHealth = 0;
            Player_Switch_Weapons();
        }

    }

    public void Player_Switch_Weapons() {
        if(mPlayerSubWeapon == null) {
            return;
        }
        GameObject tempWeapon = mPlayerWeapon.gameObject;
        mPlayerWeapon = mPlayerSubWeapon;
        mPlayerSubWeapon = tempWeapon.GetComponent<Weapon>();
        mPlayerWeapon.gameObject.SetActive(true);
        mPlayerSubWeapon.gameObject.SetActive(false);
    }

    public bool Player_Near_Resource(){

        Vector3 rayOrigin = mCam.transform.position;
        RaycastHit[] hits;

        hits = Physics.RaycastAll(rayOrigin, mCam.transform.TransformDirection(Vector3.forward), 3.0f);

        foreach(RaycastHit hit in hits){

            if (hit.collider.CompareTag("Resource")){
                Renderer outline = hit.collider.GetComponent<Renderer>();

                //We are already collecting or node is empty
                if (mIsCollecting || !hit.collider.GetComponent<ResourceNode>().enabled) {
                    mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Resource_Text("");
                    Set_Shader_Outline(outline, _propblock, 1.0f);
                } 
                else {
                    mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Resource_Text(("Press F To Collect " + hit.collider.GetComponent<ResourceNode>().Get_Name()));
                    Set_Shader_Outline(outline, _propblock, 1.02f);
                }
                _targetNode = hit.collider.GetComponent<ResourceNode>();
                return true;
            }
            
        }

        if(_targetNode != null) {
            Set_Shader_Outline(_targetNode.gameObject.GetComponent<Renderer>(), _propblock, 1.0f);
        }

        mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Resource_Text("");
        mIsCollecting = false;
        _targetNode = null;
        return false;
    }

    public bool Player_Near_Workbench() {

        Vector3 rayOrigin = mCam.transform.position;
        RaycastHit[] hits;

        hits = Physics.RaycastAll(rayOrigin, mCam.transform.TransformDirection(Vector3.forward), 3.0f);
        foreach (RaycastHit hit in hits) {

            if (hit.collider.CompareTag("Workbench")) {
                _lastWorkBench = hit.collider.gameObject;
                Renderer outline = _lastWorkBench.GetComponent<Renderer>();
                if (mIsCrafting) {
                    mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Craft_Text("");
                    Set_Shader_Outline(outline, _propblock, 1.0f);
                    return false;
                }
                else {
                    mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Craft_Text(("Press F To Craft"));
                    Set_Shader_Outline(outline, _propblock, 1.02f);
                    return true;
                }
            }
        }

        if(_lastWorkBench != null) {
            Set_Shader_Outline(_lastWorkBench.GetComponent<Renderer>(), _propblock, 1.0f);
        }
        mPlayerUIPrefab.GetComponent<PlayerUI>().Player_Craft_Text("");
        mIsCrafting = false;
        return false;
    }

    private void Set_Shader_Outline(Renderer outline, MaterialPropertyBlock propblock, float value) {

        outline.GetPropertyBlock(propblock);
        propblock.SetFloat("_OutlineWidth", value);
        outline.SetPropertyBlock(propblock);
    }
    public void Player_Gather_Resource() {
        if (Time.time > mTimeToNextCollect) {
            mTimeToNextCollect = Time.time + mCollectRate;
            if(_targetNode.Get_Resource_Count() <= 0) {
                return;
            }
            mResourceDict[_targetNode.Get_Name()] += _targetNode.Give_Resource();
            
        }
    }

    #region RPC_Functions

    [PunRPC]
    public void Take_Damage(float damage) {

        mCurrentHealth -= damage;

        if(mCurrentHealth < 0) {
            mCurrentHealth = 0;
        }
    }

    [PunRPC]
    public void Play_Particle_Projectile() {
        mPlayerWeapon.mParticleProjectile.mParticles.Play();
    }

    [PunRPC]
    public void Stop_Particle_Projectile() {
        mPlayerWeapon.mParticleProjectile.mParticles.Stop();
    }

    [PunRPC]
    public void Play_Particle_System() {
        mPlayerWeapon.mParticleSystem.Play();
    }

    [PunRPC]
    public void Stop_Particle_System() {
        mPlayerWeapon.mParticleSystem.Stop();
    }

    [PunRPC]
    public void Player_Die() {
        mArmsMesh.SetActive(false);
        mFullBodyMesh.SetActive(false);
        mRagdoll.SetActive(true);
        mPlayerWeapon.mCam.transform.Translate(0, 0, -5);
        mPlayerWeapon.gameObject.SetActive(false);
        mIsDead = true;
    }

    #endregion
}
