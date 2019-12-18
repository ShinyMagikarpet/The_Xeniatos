using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {

    

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
        Crafting,
        Trapping
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
    private ObjectPool mObjectPool;
    private GameObject mTrap;
    private MaterialPropertyBlock _propblock;
    public static Player mLocalPlayer;

    [SerializeField] private Camera mCam;
    [SerializeField] private Camera mMinimapCam;
    [SerializeField] private Camera mEffectsCamera;
    public bool IsWeeb = false;
    public int playerTeamNum;
    public GameObject[] mPlayerWeapons;

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
            this.name = photonView.Owner.NickName;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //Hide the cursor
        state = PlayerState.Idle;
        //mFullBodyMesh = Get_Player_Mesh();
        this.name = PhotonNetwork.NickName;
        mResourceDict = new Dictionary<string, int>();
        mResourceDict.Add("Iron", 200);
        mResourceDict.Add("Stone", 200);
        mResourceDict.Add("Wood", 200);
        mPlayerUIPrefab = Instantiate(mPlayerUIPrefab);
        mPlayerUIPrefab.GetComponent<PlayerUI>().SetTarget(this);
        _propblock = new MaterialPropertyBlock();
        mObjectPool = ObjectPool.Instance;
        mLocalPlayer = this;
    }

    // Update is called once per frame
    void Update(){
        
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (mIsCollecting) {
            Player_Gather_Resource();
        }

        //TODO: Needs better 3rd person camera to better show rigidbody
        if(mCurrentHealth <= 0 && !mIsDead) {
            if (PhotonNetwork.IsConnected) {
                photonView.RPC("Player_Die", RpcTarget.AllBuffered);
            }
            else {
                Player_Die();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            transform.position = Vector3.zero;
        }

        if(state == PlayerState.Trapping) {
            Player_Visual_Trap();
        }

        Player_Inputs();

    }

    void Player_Inputs() {

        //Debug.DrawRay(mPlayerWeapon.mCam.transform.position, mPlayerWeapon.mCam.transform.TransformDirection(Vector3.forward) * 3, Color.cyan);

        //The player should be allowed to access the main menu when dead
        //Menu
        if (Input.GetButtonDown("Cancel")) {

            if (state == PlayerState.Crafting) {
                state = PlayerState.Idle;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Craft_Menu();
            }
            else if (state != PlayerState.InMenu) {
                //Debug.Log("Entering menu");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                state = PlayerState.InMenu;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Pause_Menu();
            }
            else {
                Debug.Log("Leaving menu");
                state = PlayerState.Idle;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Pause_Menu();
            }

        }

        if (mIsDead && Input.GetKeyDown(KeyCode.U)) {
            if (PhotonNetwork.IsConnected)
                photonView.RPC("Player_Respawn", RpcTarget.AllBuffered);
            else
                Player_Respawn();

        }

        if (mIsDead) {
            return;
        }

        if (Player_Near_Resource() && Input.GetButtonDown("Interact"))
        {
            mIsCollecting = true;
        }

        if (Player_Near_Workbench() && Input.GetButtonDown("Interact")) {
            state = PlayerState.Crafting;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mPlayerUIPrefab.GetComponent<PlayerUI>().Use_Craft_Menu();
            mIsCrafting = true;
        }

        if(state == PlayerState.Crafting) {
            return;
        }

        //Placing Trap
        if(Input.GetButtonDown("Fire1") && state == PlayerState.Trapping) {

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
            if (PhotonNetwork.IsConnected)
                photonView.RPC("Stop_Particle_Projectile", RpcTarget.AllBuffered);
            else
                Stop_Particle_Projectile();
        }

        if(mPlayerWeapon.mIsBeam && mPlayerWeapon.mParticleSystem.isEmitting && !Input.GetButton("Fire1")) {

            if (PhotonNetwork.IsConnected)
                photonView.RPC("Stop_Particle_System", RpcTarget.AllBuffered);
            else
                Stop_Particle_System();
        }

        if (Input.GetButtonUp("Fire1") && (mPlayerWeapon.mFire_Type == Weapon.Fire_Type.fully_Auto || mPlayerWeapon.mFire_Type == Weapon.Fire_Type.single) && mPlayerWeapon.mParticleSystem != null && mPlayerWeapon.mParticleSystem.isEmitting) {
            mPlayerWeapon.mParticleSystem.Stop();
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0) {
            if (PhotonNetwork.IsConnected && this.mPlayerSubWeapon != null)
                photonView.RPC("Player_Switch_Weapons", RpcTarget.AllBuffered);
            else
                Player_Switch_Weapons();
        }

        if (Input.GetButtonDown("Trap")) {

            if (state != PlayerState.Trapping) {
                state = PlayerState.Trapping;
            }
            else {
                state = PlayerState.Idle;
            }
        }

        if (Input.GetButtonDown("Fire2")) {
            if(state != PlayerState.Sprinting && state != PlayerState.Idle)
                state = PlayerState.Idle;
        }

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

    public void Player_Visual_Trap() {

        
        if (!mTrap) {
            mTrap = mObjectPool.SpawnFromPool("Freeze Trap");
            if (!mTrap) {
                return;
            }
            mTrap.GetComponent<Collider>().enabled = false;
        }
        Vector3 rayOrigin = mCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        Debug.DrawRay(rayOrigin, mCam.transform.TransformDirection(Vector3.forward) * 3, Color.cyan);
        RaycastHit ray;
        if (Physics.Raycast(rayOrigin, mCam.transform.TransformDirection(Vector3.forward), out ray, 3)) {
            if (ray.collider.CompareTag("Ground")) {
                Debug.Log("Placing trap on ground at " + ray.point);
                mTrap.transform.position = ray.point;
                mTrap.transform.forward = transform.forward;
            }
            
        }
        else {
            mTrap.transform.forward = mCam.transform.TransformDirection(Vector3.forward);
            mTrap.transform.position = rayOrigin + mCam.transform.TransformDirection(Vector3.forward).normalized * 3;
        }
    }

    public Player Get_Local_Player() {
        return mLocalPlayer;
    }

    public Camera Get_Effects_Camera() {
        return mEffectsCamera;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        //Debug.Log("tagobject is of type " + info.Sender.TagObject.GetType());
        //Debug.Log("This object was already processed");
        //return;
        if(info.Sender.TagObject == null) {
            Debug.LogError("Tag shouldn't be null here");
            return;
        }
        //TODO: extract int array from tagobject
        //this.playerTeamNum = (int)info.Sender.TagObject;   //The player's tagobject is set from the matchmaking lobby
        int[] matchInfo = info.Sender.TagObject as int[];
        this.playerTeamNum = matchInfo[0];
        transform.SetParent(PlayerManager.Instance.transform);
        PlayerManager.Instance.Add_Player(this);
        info.Sender.TagObject = this.gameObject;
        PlayerManager.Instance.Put_Player_On_Team(this);
        
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
        mIsDead = true;
        state = PlayerState.Dead;
        GetComponent<CharacterController>().enabled = false;
        mArmsMesh.SetActive(false);
        mFullBodyMesh.SetActive(false);
        mRagdoll.SetActive(true);
        mPlayerWeapon.mCam.transform.Translate(0, 0, -2);
        mPlayerWeapon.gameObject.SetActive(false);

    }

    [PunRPC]
    public void Player_Respawn() {
        GetComponent<CharacterController>().enabled = true;
        if (PhotonNetwork.IsConnected && !photonView.IsMine) {
            mArmsMesh.SetActive(false);
            mFullBodyMesh.SetActive(true);
        }
        else {
            mArmsMesh.SetActive(true);
        }
        mRagdoll.SetActive(false);
        mPlayerWeapon.mCam.transform.Translate(0, 0, 2);
        mPlayerWeapon.gameObject.SetActive(true);
        mIsDead = false;
        state = PlayerState.Idle;
        mCurrentHealth = mMaxHealth;
    }

    [PunRPC]
    public void Player_Switch_Weapons() {
        if (mPlayerSubWeapon == null) {
            return;
        }
        GameObject tempWeapon = mPlayerWeapon.gameObject;
        mPlayerWeapon = mPlayerSubWeapon;
        mPlayerSubWeapon = tempWeapon.GetComponent<Weapon>();
        mPlayerWeapon.gameObject.SetActive(true);
        mPlayerSubWeapon.gameObject.SetActive(false);
    }

    [PunRPC]
    public void Player_Craft_Weapon(int weaponindex, string weaponName) {

        if (mPlayerSubWeapon == null) {
            mPlayerSubWeapon = mPlayerWeapons[weaponindex].GetComponent<Weapon>();
            mPlayerWeapons[weaponindex].SetActive(true);
            Player_Switch_Weapons();
        }
        else {
            mPlayerWeapon.gameObject.SetActive(false);
            mPlayerWeapon = mPlayerWeapons[weaponindex].GetComponent<Weapon>();
            mPlayerWeapons[weaponindex].SetActive(true);
        }
       mPlayerWeapon.Weapon_Setup();

        if (photonView.IsMine) {
            mResourceDict["Iron"] -= WeaponRecipes.gWeaponRecipes[weaponName][0];
            mResourceDict["Stone"] -= WeaponRecipes.gWeaponRecipes[weaponName][1];
            mResourceDict["Wood"] -= WeaponRecipes.gWeaponRecipes[weaponName][2];
        }
    }

    #endregion
}
