using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    private Vector3 movement = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);
        movement *= speed * Time.deltaTime;
        Debug.Log("Trying to move forward");

        controller.Move(movement);
        
    }
}
