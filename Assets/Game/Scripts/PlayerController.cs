using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 2f;

    private CharacterController characterController;

	void Start ()
    {
        characterController = GetComponent<CharacterController>();
	}
	
	void Update ()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveOffset = new Vector3(horizontal, 0, vertical) * Speed;

        characterController.SimpleMove(moveOffset);
	}
}
