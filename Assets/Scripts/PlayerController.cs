using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    public float dashSpeed;
    private bool isDashing;
    public float airSpeed;
    public float jump_speed;
    private float gravity = 6.0f;

    //Mouse controls
    public float mouseSensitivity;
    public float mouseHorizontal;
    public float mouseVertical;
    public float lookUpRange = 70.0f;
    private Vector3 movement = Vector3.zero;

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {

        mouseHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseHorizontal, 0);

        mouseVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseVertical = Mathf.Clamp(mouseVertical, -lookUpRange, lookUpRange);
        cam.transform.localRotation = Quaternion.Euler(mouseVertical, 0, 0);


        //Ground Movement
        if (controller.isGrounded) {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            movement = transform.TransformDirection(movement);
            movement *= speed;
            if (Input.GetButton("Jump")) {
                movement.y = jump_speed;
            }
        }

        //Air Movement
        if (!controller.isGrounded) {
            //movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            //movement = transform.TransformDirection(movement);
            //movement *= airSpeed;
            //Debug.Log("Trying to do air movement");

        }

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

        movement.y -= gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);
        
    }
}
