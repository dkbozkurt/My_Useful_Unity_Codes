using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Movement on x-z axis with Joystick
// Works with "Joystick Pack" asset
/// <summary>
/// Attach script to main player that gonna be control by joystick or wasd on x-z axis
/// Use this script with "Joystick Pack" asset and,"_fixedJoystick" variable must refer
/// the script that attached to the Joystick prefab coming from the asset.
/// </summary>

public class MovementByJoystick : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private FixedJoystick _fixedJoystick;

    private void Start()
    {
        // Can be reachable by using [SerializeField]
        _fixedJoystick = FindObjectOfType<FixedJoystick>();
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = new Vector3(_fixedJoystick.Horizontal +Input.GetAxis("Horizontal"),
                                    0,
                                    _fixedJoystick.Vertical +Input.GetAxis("Vertical"));  //
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}