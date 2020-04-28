using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GravitagMovementHandler : NetworkBehaviour
{
    #region Variables
    [Header("Character stuff")]
    [SerializeField] private Camera camera;
    [SerializeField] private CapsuleCollider collider;
    [SerializeField] private Rigidbody rigidbody;
    public KeyBindings bindings;
    public PlayerConfig settings;

    [Header("Vars")]
    // Camera setup
    private float viewOffset = 1.65f; // The height at which the camera is bound to
    private float camXRotation;
    private float xMouseSensitivity;
    private float yMouseSensitivity;

    [SerializeField] private float gravity;
    [SerializeField] private float friction; //Ground friction
    [SerializeField] private float stopSpeed;

    /* Movement stuff */
    [SerializeField] private float moveSpeed;       // Ground move speed
    [SerializeField] private float accel; // Ground accel
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpForce;       // The speed at which the character's up axis gains when hitting jump
    private int jumps;                              // The number of jumps the character has
    private int maxJumps;                           // The number of jumps to reset to

    [SerializeField] private Vector3 velocity;
    private bool isGrounded = false;
    private Vector3 preboost;
    [SerializeField] private float boosted;
    private Vector3 wishMove = Vector3.zero;
    #endregion Variables
    #region Mirror
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main.orthographic = false;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = rigidbody.position + new Vector3(0f, viewOffset, 0f);
        Camera.main.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
        camera = Camera.main;

        if (!collider)
            collider = GetComponent<CapsuleCollider>();
    }

    void OnDisable()
    {
        if (isLocalPlayer)
        {
            Camera.main.orthographic = true;
            Camera.main.transform.SetParent(null);
            Camera.main.transform.localPosition = new Vector3(0f, 70f, 0f);
            Camera.main.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }
    #endregion Mirror

    #region Unity Functions
    private void Awake()
    {
        // Initialize vars
        rigidbody.freezeRotation = true;
        camXRotation = 0.0f;
        xMouseSensitivity = 30.0f;
        yMouseSensitivity = 30.0f;

        gravity = 20.0f;
        friction = 5f;
        stopSpeed = 5.0f;

        moveSpeed = 10f;
        accel = 5f;
        airSpeed = 2f;
        jumpForce = 10.0f;
        jumps = 2;
        maxJumps = 2;

        boosted = 0f;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Cursor.visible == false)
        {
            /* Camera rotation stuff, mouse controls this shit */
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * settings.mouseXSensitivity * 10;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * settings.mouseYSensitivity * 10;

            camXRotation -= mouseY;
            camXRotation = Mathf.Clamp(camXRotation, -90f, 90f);

            Camera.main.transform.localRotation = Quaternion.Euler(camXRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        if (boosted > 0)
        {
            boosted -= Time.deltaTime;
            if (boosted <= 0)
                velocity = preboost;
            else
                return;
        }
        if (isGrounded)
            GroundMove();
        else if (!isGrounded)
            AirMove();

        AbilityInputs();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = velocity;
    }

    #region Collider Triggers 
    private void OnCollisionEnter()
    {
        isGrounded = true;
        jumps = maxJumps;
        if (Input.GetKey(bindings.Jump))
        {
            velocity.y = jumpForce;
            jumps = maxJumps - 1;
        }
    }

    private void OnCollisionStay()
    {
        isGrounded = true;
        jumps = maxJumps;
    }

    private void OnCollisionExit()
    {
        isGrounded = false;
    }
    #endregion Collider Trigger
    #endregion Unity Fucntions

    #region Player Movement
    #region Player Input
    private void GroundMove()
    {
        float x = 0f, z = 0f;
        if (Cursor.visible == false)
        {
            if (Input.GetKey(bindings.Forward)) z += 1;
            if (Input.GetKey(bindings.Backward)) z -= 1;
            if (Input.GetKey(bindings.Right)) x += 1;
            if (Input.GetKey(bindings.Left)) x -= 1;
        }
        wishMove = x * transform.right + z * transform.forward;
        if (Input.GetKeyDown(bindings.Jump) && jumps > 0 && Cursor.visible == false)
        {
            velocity.y = jumpForce;
            jumps--;
        }
        else
            Friction();
        Accelerate(moveSpeed);
    }

    private void AirMove()
    {
        float x = 0f, z = 0f;
        if (Cursor.visible == false)
        {
            if (Input.GetKey(bindings.Forward)) z += 1;
            if (Input.GetKey(bindings.Backward)) z -= 1;
            if (Input.GetKey(bindings.Right)) x += 1;
            if (Input.GetKey(bindings.Left)) x -= 1;
        }
        wishMove = x * transform.right + z * transform.forward;
        if (Input.GetKeyDown(bindings.Jump) && jumps > 0 && Cursor.visible == false)
        {
            velocity.y = jumpForce;
            jumps--;
        }
        velocity.y -= gravity * Time.deltaTime;
        Accelerate(airSpeed);
    }

    private void AbilityInputs()
    {
        if (Input.GetMouseButtonDown(0) && boosted <= 0 && Cursor.visible == false)
        {
            preboost = velocity;
            velocity = (Camera.main.transform.forward * 50);
            boosted = 0.25f;
        }
    }
    #endregion Player Input

    #region Physics
    private void Friction()
    {
        var speed = velocity.magnitude;
        if (speed > 0)
        {
            var control = speed < stopSpeed ? stopSpeed : speed;
            var drop = control * friction * Time.deltaTime;
            var newSpeed = speed - drop;
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }
            newSpeed /= speed;
            velocity *= newSpeed;
        }
    }

    private void Accelerate(float speed)
    {
        var vel = new Vector2(velocity.x, velocity.z);
        var wishDir = new Vector2(wishMove.x, wishMove.z).normalized;
        var currentSpeed = Vector2.Dot(vel, wishDir);
        var addSpeed = speed - currentSpeed;
        addSpeed = Mathf.Max(Mathf.Min(addSpeed, accel * Time.deltaTime * speed), 0);
        velocity.x += addSpeed * wishDir.x;
        velocity.z += addSpeed * wishDir.y;
    }
    #endregion Physics
    #endregion Player Movement

    #region External Interactions
    /// <summary>
    /// Applies a force in the forward direction of the player
    /// </summary>
    /// <param name="inForce">The strength of the force acting on the player</param>
    /// <param name="useLookDirection">Default: False uses camera's look direction instead of true forward</param>
    public void ApplyForce(float inForce, bool useLookDirection = false)
    {
        Debug.Log(transform.position);
        if (useLookDirection)
        {
            var f = Camera.main.transform.forward * inForce;
            rigidbody.AddForce(new Vector3(f.x, 0.3f * inForce, f.z), ForceMode.Impulse);
        }
        else
            rigidbody.AddRelativeForce(Vector3.forward * inForce, ForceMode.Impulse);
        Debug.Log(transform.position);
    }

    /// <summary>
    /// Applies a force to the player
    /// </summary>
    /// <param name="inForce">The strength and direction of the force acting on the player</param>
    public void ApplyForce(Vector3 inForce)
    {
        rigidbody.AddRelativeForce(inForce, ForceMode.Impulse);
    }
    #endregion External Interactions
}
