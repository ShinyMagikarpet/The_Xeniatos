using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private CharacterController controller;
    public float speed;
    public float dashSpeed;
    private bool isDashing;
    public float airSpeed;
    public float jumpSpeed;
    private float gravity = -9.81f;
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
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement

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

        verticalVelocity += gravity * Time.deltaTime;
        
        
        if(controller.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
        }

        movement = new Vector3(sideMovement, verticalVelocity, forwardMovement);

        if (Input.GetButtonDown("Dash") && !isDashing) {
            isDashing = true;
        }

        if(isDashing && Input.GetAxis("Vertical") <= 0) {
            isDashing = false;
        }

        if ((Input.GetKeyUp(KeyCode.W) && isDashing)) {
            isDashing = false;
        }

        if (isDashing && controller.isGrounded) {
            movement *= dashSpeed;
        }

        movement = transform.rotation * movement;

        controller.Move(movement * Time.deltaTime);
        #endregion
    }
}
