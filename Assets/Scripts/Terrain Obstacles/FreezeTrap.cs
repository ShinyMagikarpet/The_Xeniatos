using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour{

    private float freezeTime = 10.0f;
    private bool isFreezingPlayer = false;
    private Player weeb;
    [SerializeField] private Material[] playerMats;
    private void OnTriggerEnter(Collider other) {

        if (isFreezingPlayer) return;

        if (other.CompareTag("Player")) {
            isFreezingPlayer = true;
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.speed = 0.0f;
            controller.jumpSpeed = 0.0f;
            Player[] players = FindObjectsOfType<Player>();
            foreach(Player player in players) {
                if (player.IsWeeb) {
                    weeb = player;
                    break;
                }
            }
            SkinnedMeshRenderer[] meshes = other.GetComponentsInChildren<SkinnedMeshRenderer>();
            StartCoroutine(FreezePlayer(freezeTime, controller, meshes));
        }
    }

    IEnumerator FreezePlayer(float timer, PlayerController playerController, SkinnedMeshRenderer[] meshes) {
        FreezePlayerShader(meshes);
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
        weeb.Get_Effects_Camera().GetComponent<CameraFrozen>().enabled = true;
    }

    void UnfreezePlayerShader(SkinnedMeshRenderer[] meshes) {
        foreach (SkinnedMeshRenderer mesh in meshes) {
            for (int i = 0; i < mesh.materials.Length; i++) {
                mesh.materials[i].SetOverrideTag("IceTrap", "False");
            }
        }
        weeb.Get_Effects_Camera().GetComponent<CameraFrozen>().enabled = false;
    }
    
}
