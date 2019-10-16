using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    #region Variables
    private CharacterController controller;
    public float speed;
    public float dashSpeed;
    private bool isDashing;
    public float airSpeed;
    public float jumpSpeed;
    [SerializeField] private float gravity = -12f;
    private float verticalVelocity = 0;

    //Mouse controls
    public float mouseSensitivity;
    public float mouseHorizontal;
    public float mouseVertical;
    public float lookUpRange = 70.0f;
    private Vector3 movement = Vector3.zero;

    public Camera cam;
    private Player mPlayer;
    #endregion

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        mPlayer = GetComponent<Player>();
        isDashing = false;
        if (!photonView.IsMine && PhotonNetwork.IsConnected == true) {
            cam.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement


        if (!photonView.IsMine && PhotonNetwork.IsConnected == true) {
            return;
        }

        if(mPlayer.state == Player.PlayerState.InMenu) {
            return;
        }

        mouseHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseHorizontal, 0);

        mouseVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseVertical = Mathf.Clamp(mouseVertical, -lookUpRange, lookUpRange);
        cam.transform.localRotation = Quaternion.Euler(mouseVertical, 0, 0);

        float forwardMovement = Input.GetAxis("Vertical") * speed;
        float sideMovement = Input.GetAxis("Horizontal") * speed;

        if(!controller.isGrounded)
            verticalVelocity += gravity * Time.deltaTime;
        
        
        if(controller.isGrounded && Input.GetButtonDown("Jump")) {
            if (isDashing) {
                verticalVelocity = jumpSpeed / 1.6f;
            } 
            else {
                verticalVelocity = jumpSpeed;
            }
        }

        movement = new Vector3(sideMovement, verticalVelocity, forwardMovement);

        if (Input.GetButtonDown("Dash") && !isDashing) {
            isDashing = true;
        }

        if(isDashing && Input.GetAxis("Vertical") <= 0 && controller.isGrounded) {
            isDashing = false;
        }

        if ((Input.GetKeyUp(KeyCode.W) && isDashing && controller.isGrounded)) {
            isDashing = false;
        }

        if (isDashing) {
            movement *= dashSpeed;
        }

        movement = transform.rotation * movement;

        

        controller.Move(movement * Time.deltaTime);
        #endregion
    }
}
