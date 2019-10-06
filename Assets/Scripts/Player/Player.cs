using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
        InMenu
    };

    [HideInInspector]
    public bool mIsDead = false;                    
    public int mMaxHealth = 100;
    public int mCurrentHealth = 100;
    public Weapon mPlayerWeapon;
    public Text ammoText;
    public Text healthText;
    public Text collectText;
    public PlayerState state;
    public GameObject mMenu;
    public GameObject mFullBodyMesh;
    public Dictionary<string, int> mResourceDict;

    void Start()
    {
        mPlayerWeapon = GetComponentInChildren<Weapon>();
        mPlayerWeapon.mCam = GetComponentInChildren<Camera>();
        mPlayerWeapon.mOwner = this;
        state = PlayerState.Idle;
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        mFullBodyMesh = Get_Player_Mesh(); 
        this.name = PhotonNetwork.NickName;
        mMenu.SetActive(false);
        mResourceDict = new Dictionary<string, int>();
        mResourceDict.Add("Iron", 0);
        mResourceDict.Add("Stone", 0);
        mResourceDict.Add("Wood", 0);

    }

    // Update is called once per frame
    void Update()
    {

        ammoText.text = mPlayerWeapon.mAmmoLoaded.ToString() + '/' +
            mPlayerWeapon.mAmmoHeld.ToString();

        healthText.text = mCurrentHealth.ToString() + '/' + mMaxHealth.ToString();


        if (photonView.IsMine) {
            Player_Inputs();
        }

        /*
         * If the weapon is owned, tag it for player
        if (mPlayerWeapon.mOwner) {
            mPlayerWeapon.tag = "Player";
        }
        */


        Player_Inputs();


        Player_Near_Object();



    }

    void Player_Inputs() {

        Debug.DrawRay(mPlayerWeapon.mCam.transform.position, mPlayerWeapon.mCam.transform.TransformDirection(Vector3.forward) * 3, Color.cyan);

        //Menu
        if (Input.GetButtonDown("Cancel")) {

            if (state != PlayerState.InMenu) {
                Debug.Log("Entering menu");
                state = PlayerState.InMenu;
                mMenu.SetActive(true);
            } else {
                Debug.Log("Leaving menu");
                state = PlayerState.Idle;
                mMenu.SetActive(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!mFullBodyMesh.activeInHierarchy)
            {
                mFullBodyMesh.SetActive(true);
            }
            else
            {
                mFullBodyMesh.SetActive(false);
            }
        }

        //Single fire
        if (Input.GetButtonDown("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.single && state != PlayerState.InMenu) {
            mPlayerWeapon.Fire_Weapon();
        }

        //Automatic fire
        if (Input.GetButton("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.fully_Auto && state != PlayerState.InMenu) {
            Debug.Log("Fire fully auto");
            mPlayerWeapon.Fire_Weapon();
        }

        //Reload
        if (Input.GetButtonDown("Reload") || mPlayerWeapon.mAmmoLoaded == 0 && state != PlayerState.InMenu) {
            mPlayerWeapon.Reload_Weapon();
        }

    }

    public void Player_Near_Object()
    {

        Vector3 rayOrigin = mPlayerWeapon.mCam.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, mPlayerWeapon.mCam.transform.forward, out hit, 3.0f))
        {
            if (hit.collider.CompareTag("Resource"))
            {
                collectText.gameObject.SetActive(true);
                collectText.text = ("Press F To Collect " + hit.collider.GetComponent<ResourceNode>().Get_Name());
                return;
            }
        }
        else
        {
            collectText.gameObject.SetActive(false);
        }

    }

    GameObject Get_Player_Mesh(){

        GameObject mesh = null;

        for(int i = 0; i < transform.childCount; i++){
            if (transform.GetChild(i).CompareTag("Mesh")){
                mesh = transform.GetChild(i).gameObject;
            }
        }

        if(mesh == null){
            Debug.LogError("Mesh is null");
        }
        return mesh;

    }

}
