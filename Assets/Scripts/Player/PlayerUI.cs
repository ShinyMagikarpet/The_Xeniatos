using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Text playerAmmoText;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text playerActionText;
    [SerializeField] private Text playerIronText;
    [SerializeField] private Text playerStoneText;
    [SerializeField] private Text playerWoodText;
    [SerializeField] private GameObject playerPauseMenu;
    [SerializeField] private GameObject playerMiniMap;
    private Player target;

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    private void Start() {
        playerPauseMenu = Instantiate(playerPauseMenu, GameObject.Find("Canvas").transform);
        playerMiniMap = Instantiate(playerMiniMap, GameObject.Find("Canvas").transform);
        playerPauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) {
            Destroy(this.gameObject);
            return;
        }

        if(playerAmmoText != null) {
            playerAmmoText.text = target.mPlayerWeapon.mAmmoLoaded.ToString() + '/' +
            target.mPlayerWeapon.mAmmoHeld.ToString();
        }

        if (playerHealthText != null) {
            int healthRounded = (int)Mathf.Round(target.mCurrentHealth);
            playerHealthText.text = healthRounded.ToString() + '/' + target.mMaxHealth.ToString();
        }

        if (playerIronText != null) {
            playerIronText.text = "x " + target.mResourceDict["Iron"].ToString();
        }

        if (playerStoneText != null) {
            playerStoneText.text = "x " + target.mResourceDict["Stone"].ToString();
        }

        if (playerWoodText != null) {
            playerWoodText.text = "x " + target.mResourceDict["Wood"].ToString();
        }
    }

    public void SetTarget(Player _target) {

        if(_target == null) {
            Debug.Log("player is null");
            return;
        }

        target = _target;

    }

    public void Use_Pause_Menu() {

        if (!playerPauseMenu.activeSelf) {
            playerPauseMenu.SetActive(true);
        } 
        else {
            playerPauseMenu.SetActive(false);
        }
    }

    public void Player_Near_Resource_Text(string message) {
        playerActionText.text = message;
    }
}
