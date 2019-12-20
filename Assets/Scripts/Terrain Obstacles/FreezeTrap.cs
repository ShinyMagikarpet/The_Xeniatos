using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTrap : MonoBehaviour{

    private float freezeTime = 10.0f;
    private bool isFreezingPlayer = false;
    [SerializeField]private Player weeb;
    Image freezeOverlay;

    private void Start() {
        if(!weeb)
            GetWeeb();

        //if (!freezeOverlay)
        //    freezeOverlay = GetFreezeImage();
        
    }
    private void OnTriggerEnter(Collider other) {

        if (isFreezingPlayer) return;

        if (other.CompareTag("Player")) {
            isFreezingPlayer = true;
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed = 0.0f;
            controller.jumpSpeed = 0.0f;
            
            SkinnedMeshRenderer[] meshes = other.GetComponentsInChildren<SkinnedMeshRenderer>();
            StartCoroutine(FreezePlayer(freezeTime, controller, meshes));
        }
    }

    IEnumerator FreezePlayer(float timer, PlayerController playerController, SkinnedMeshRenderer[] meshes) {
        FreezePlayerShader(meshes);
        //if (!freezeOverlay)
        //    freezeOverlay = GetFreezeImage();
        //freezeOverlay.gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        playerController.speed = playerController.GetPlayerStartSpeed();
        playerController.jumpSpeed = playerController.GetPlayerStartJumpSpeed();
        UnfreezePlayerShader(meshes);
        isFreezingPlayer = false;
        gameObject.SetActive(false);
    }

    void FreezePlayerShader(SkinnedMeshRenderer[] meshes) {
        foreach (SkinnedMeshRenderer mesh in meshes) {
            for (int i = 0; i < mesh.materials.Length; i++) {
                mesh.materials[i].SetOverrideTag("IceTrap", "True");
            }
        }
        if (!weeb) GetWeeb();
            weeb.Get_Effects_Camera().GetComponent<CameraFrozen>().enabled = true;
        //freezeOverlay.gameObject.SetActive(false);
    }

    void UnfreezePlayerShader(SkinnedMeshRenderer[] meshes) {
        foreach (SkinnedMeshRenderer mesh in meshes) {
            for (int i = 0; i < mesh.materials.Length; i++) {
                mesh.materials[i].SetOverrideTag("IceTrap", "False");
            }
        }
        if (!weeb) GetWeeb();
            weeb.Get_Effects_Camera().GetComponent<CameraFrozen>().enabled = false;

        if (GetComponent<MeshRenderer>().enabled == false) {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void FreezeAllPlayers() {
        List<Player> players = PlayerManager.Instance.Get_Players_Team2();
        if (GetComponent<MeshRenderer>().enabled == true) {
            GetComponent<MeshRenderer>().enabled = false;
        }
        foreach(Player player in players) {
            PlayerController controller = player.GetComponent<PlayerController>();
            controller.speed = 0.0f;
            controller.jumpSpeed = 0.0f;

            SkinnedMeshRenderer[] meshes = player.GetComponentsInChildren<SkinnedMeshRenderer>();
            StartCoroutine(FreezePlayer(freezeTime, controller, meshes));
        }
    }

    private void GetWeeb() {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players) {
            if (player.IsWeeb) {
                weeb = player;
                break;
            }
        }
    }

    private Image GetFreezeImage() {
        Image image = GameObject.Find("FrozenOverlay").GetComponent<Image>();
        if (image)
            return image;
        else
            return null;
    }

}
