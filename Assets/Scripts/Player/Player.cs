using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviour
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
    public PlayerState state;
    public GameObject mMenu;

    void Start()
    {
        mPlayerWeapon = GetComponentInChildren<Weapon>();
        mPlayerWeapon.mCam = GetComponentInChildren<Camera>();
        mPlayerWeapon.mOwner = this;
        state = PlayerState.Idle;
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        Debug.Log("Nick name is " + PhotonNetwork.NickName);
        name = PhotonNetwork.NickName;
        mMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        ammoText.text = mPlayerWeapon.mAmmoLoaded.ToString() + '/' +
            mPlayerWeapon.mAmmoHeld.ToString();

        healthText.text = mCurrentHealth.ToString() + '/' + mMaxHealth.ToString();

        if (Input.GetButtonDown("Cancel")) {

            if(state != PlayerState.InMenu) {
                Debug.Log("Entering menu");
                state = PlayerState.InMenu;
                mMenu.SetActive(true);
            } else {
                Debug.Log("Leaving menu");
                state = PlayerState.Idle;
                mMenu.SetActive(false);
            }

        }

        if (Input.GetButtonDown("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.single && state != PlayerState.InMenu) {
            mPlayerWeapon.Fire_Weapon();
        }

        if(Input.GetButton("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.fully_Auto && state != PlayerState.InMenu) {
            Debug.Log("Fire fully auto");
            mPlayerWeapon.Fire_Weapon();
        }

        if (Input.GetButtonDown("Reload") || mPlayerWeapon.mAmmoLoaded == 0 && state != PlayerState.InMenu) {
            mPlayerWeapon.Reload_Weapon();
        }

        if (mPlayerWeapon.mOwner) {
            mPlayerWeapon.tag = "Player";
        }
        
    }
}
