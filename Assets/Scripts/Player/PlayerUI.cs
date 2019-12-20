using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Text playerAmmoText;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text playerCollectText;
    [SerializeField] private Text playerCraftText;
    [SerializeField] private Text playerIronText;
    [SerializeField] private Text playerStoneText;
    [SerializeField] private Text playerWoodText;
    [SerializeField] private GameObject playerPauseMenu;
    [SerializeField] private GameObject playerMiniMap;
    [SerializeField] private GameObject playerCraftMenu;
    [SerializeField] private GameObject woodUIObject;
    [SerializeField] private GameObject stoneUIObject;
    [SerializeField] private GameObject ironUIObject;
    private Player target;

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    private void Start() {
        playerPauseMenu = Instantiate(playerPauseMenu, GameObject.Find("Canvas").transform);
        playerMiniMap = Instantiate(playerMiniMap, GameObject.Find("Canvas").transform);
        playerCraftMenu = Instantiate(playerCraftMenu, GameObject.Find("Canvas").transform);
        playerCraftMenu.GetComponent<CraftMenuButton>().playerUI = this;
        playerPauseMenu.SetActive(false);
        playerCraftMenu.SetActive(false);
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

        if (!GameManager.Instance.IsTeamMode()) {
            if (_target.IsWeeb) {
                playerMiniMap.SetActive(false);
                playerHealthText.gameObject.SetActive(false);
            }
            else {
                playerMiniMap.SetActive(true);
            }

            playerCraftMenu.SetActive(false);
            woodUIObject.SetActive(false);
            stoneUIObject.SetActive(false);
            ironUIObject.SetActive(false);
            playerAmmoText.gameObject.SetActive(false);
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

    public void Use_Craft_Menu() {

        if (!playerCraftMenu.activeSelf) {
            playerCraftMenu.SetActive(true);
        }
        else {
            playerCraftMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (target.state != Player.PlayerState.Idle) {
               target.state = Player.PlayerState.Idle;
            }
        }
    }

    public void Player_Resource_Text(string message) {
        playerCollectText.text = message;
    }

    public void Player_Craft_Text(string message) {
        playerCraftText.text = message;
    }
}
