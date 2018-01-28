using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 2f;

    private CharacterController characterController;
	private GameManager gameManager;
	private SpriteRenderer spriteRenderer;

	private void Start ()
    {
        characterController = GetComponent<CharacterController>();
		gameManager = GameObject.FindGameObjectWithTag ("Director").GetComponent<GameManager> ();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		if (UnityEngine.Random.value < 0.5)
			spriteRenderer.sprite = Resources.Load<Sprite> ("Characters/players/player1");
		else
			spriteRenderer.sprite = Resources.Load<Sprite> ("Characters/players/player2");
	}
	
	private void Update ()
    {
		if (gameManager.gameStarted) {
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");

			Vector3 moveDir = calculateMoveDir (horizontal, vertical);

			characterController.SimpleMove (moveDir * Speed);
		}
	}

    private Vector3 calculateMoveDir(float horizontal, float vertical)
    {
        Camera camera = Camera.main;

        Vector3 cameraForward = Vector3.Scale(
            camera.transform.forward,
            new Vector3(1, 0, 1));

        Vector3 moveDir = (
            (vertical * cameraForward.normalized) +
            (horizontal * camera.transform.right));

        return moveDir.normalized;
    }
}
