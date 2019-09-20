using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    public float airSpeed;
    public float jump_speed;
    private float gravity = 6.0f;
    private Vector3 movement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Trying to do air movement");

        }

        movement.y -= gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);
    }
}
