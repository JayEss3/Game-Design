using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Rigidbody rigidbody;
	
	// Player state
	[SerializeField] private bool hasJump;
	[SerializeField] private bool isGrounded;
	[SerializeField] private bool wishJump;
	[SerializeField] private Vector3 wishRun;

	// Player physics
	[SerializeField] private float moveSpeed = 10f;
	// Accel, decel, and gravity. Just for consistency's sake.
	[SerializeField] private float acceleration = 20f;
	[SerializeField] private float maxFallSpeed = 50f;
	[SerializeField] private float airAcceleration = 5f;
	[SerializeField] private float airControl = 0.3f;
	[SerializeField] private float jumpForce = 10f;
	
	// Player camera
	private float rotX = 0f;
	private float rotY = 0f;
	[SerializeField] private Transform camera;

    void Awake()
    {
		if (!rigidbody) rigidbody = transform.GetComponent<Rigidbody>();
		Camera mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
		{
			isJumping = true;
			print("Jumped!");
		}
		wishRun = new Vector3(acceleration, 0, acceleration);
		if (Input.GetKey(KeyCode.W)) wishRun += transform.forward;
		if (Input.GetKey(KeyCode.A)) wishRun -= transform.right;
		if (Input.GetKey(KeyCode.S)) wishRun -= transform.forward;
		if (Input.GetKey(KeyCode.D)) wishRun += transform.right;
    }
	
	void FixedUpdate()
	{
		if (isJumping && hasJump)
		{
			if (!isGrounded) hasJump = false;
			print("Is jumping...");
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
			isJumping = false;
		}
		float downVelocity = 0f;
		if (!isGrounded) downVelocity = Mathf.Max(rigidbody.velocity.y - gravity * Time.deltaTime, -100f);
		rigidbody.velocity += new Vector3(wishRun.x, downVelocity, wishRun.z);
	}
	
	void OnCollisionEnter()
	{
		isJumping = false;
		isGrounded = true;
		hasJump = true;
	}
	
	void OnCollisionExit()
	{
		isGrounded = false; 
	}
}
