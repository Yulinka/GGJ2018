using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;

	void Start ()
    {
        characterController = GetComponent<CharacterController>();
	}
	
	void Update ()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        characterController.SimpleMove(new Vector3(horizontal, 0, vertical));
		
	}
}
