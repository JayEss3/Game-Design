using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerController : NetworkBehaviour
{
  [Header("Objects")]
  [SerializeField] private KeyBindings keyBindings;
  [SerializeField] private PlayerSettings playerSettings;
  [SerializeField] private PlayerConfig playerConfig;

  [Header("Character stuff")]
  [SerializeField] private Camera camera;
  [SerializeField] private CapsuleCollider collider;
  [SerializeField] private Rigidbody rigidbody;

  [Header("UI stuff")]
  [SerializeField] private TextMeshProUGUI playerNameplate;
  [SerializeField] private Canvas canvas;
  [SerializeField] private UserInterfaceManager userInterfaceManager;

  [Header("Vars")]
  // Camera setup
    private float viewOffset = 0.6f; // The height at which the camera is bound to
  private float camXRotation = 0.0f;
  private float xMouseSensitivity = 30.0f;
  private float yMouseSensitivity = 30.0f;

  [SerializeField] private float gravity = 20.0f;
  [SerializeField] private float friction = 5f; //Ground friction
  [SerializeField] private float stopSpeed = 5.0f;

  /* Movement stuff */
  [SerializeField] private float moveSpeed = 25f;        // Ground move speed
  [SerializeField] private float runAcceleration = 5; // Ground accel
  [SerializeField] private float airAcceleration = 2; // Air accel
  [SerializeField] private float jumpForce = 10.0f;       // The speed at which the character's up axis gains when hitting jump
  private int jumps = 2;				 // The number of jumps the character has
  private int maxJumps = 2;    		 // The number of jumps to reset to

  private Vector3 velocity;
  private bool isGrounded = false;
  private Vector3 wishMove = Vector3.zero;

  public override void OnStartLocalPlayer()
  {
    base.OnStartLocalPlayer();
    Camera.main.orthographic = false;
    Camera.main.transform.SetParent(transform);
    Camera.main.transform.localPosition = rigidbody.position + new Vector3(0f, viewOffset, 0f);
    Camera.main.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
    camera = Camera.main;

    playerNameplate.text = playerConfig.playerName;
    if (!keyBindings)
      keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
    if (!playerSettings)
      playerSettings = Resources.Load<PlayerSettings>("Objects/PlayerSettings");
    if (!playerConfig)
      playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
    if (!rigidbody)
      rigidbody = transform.GetComponent<Rigidbody>();
    if (!userInterfaceManager)
      userInterfaceManager = GameObject.Find("Canvas").GetComponent<UserInterfaceManager>();
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

  private void Awake()
  {
    if (!keyBindings)
      keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
    // Hide the cursor
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    rigidbody.freezeRotation = true;
  }

  private void OnValidate()
  {
    if (!playerConfig)
      playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
  }

  private void Update()
  {
    if (!isLocalPlayer) return;

    /* Ensure that the cursor is locked into the screen */
    if (Cursor.lockState != CursorLockMode.Locked) {
      if (Input.GetButtonDown("Fire1"))
        Cursor.lockState = CursorLockMode.Locked;
    }

    /* Camera rotation stuff, mouse controls this shit */
    var mouseX = Input.GetAxisRaw("Mouse X") * xMouseSensitivity * Time.deltaTime;
    var mouseY = Input.GetAxisRaw("Mouse Y") * yMouseSensitivity * Time.deltaTime;

    camXRotation -= mouseY;
    camXRotation = Mathf.Clamp(camXRotation, -90f, 90f);

    Camera.main.transform.localRotation = Quaternion.Euler(camXRotation, 0f, 0f);
    transform.Rotate(Vector3.up * mouseX);

    if(isGrounded)
      GroundMove();
    else if(!isGrounded)
      AirMove();
    print("Magnitude: " + new Vector2(velocity.x, velocity.z).magnitude);
  }

  private void FixedUpdate()
  {
    rigidbody.velocity = velocity;
  }

  private void OnCollisionEnter()
  {
    isGrounded = true;
    jumps = maxJumps;
    if (Input.GetButton("Jump"))
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

  private void GroundMove()
  {
    float x = 0, z = 0;
    if (Input.GetKey(keyBindings.Forward))  z += 1;
    if (Input.GetKey(keyBindings.Backward)) z -= 1;
    if (Input.GetKey(keyBindings.Right))    x += 1;
    if (Input.GetKey(keyBindings.Left))     x -= 1;
    wishMove = x * transform.right + z * transform.forward;
    if (Input.GetButtonDown("Jump") && jumps > 0)
    {
      velocity.y = jumpForce;
      jumps--;
    }
    Friction();
    Accelerate(runAcceleration);
  }

  private void AirMove()
  {
    if (Input.GetButtonDown("Jump") && jumps > 0)
    {
      velocity.y = jumpForce;
      jumps--;
    }
    Accelerate(airAcceleration);
    velocity.y -= gravity * Time.deltaTime;
  }

  private void Friction()
  {
    var speed = velocity.magnitude;
    if (speed > 0)
    {
      var control = speed < stopSpeed ? stopSpeed : speed;
      var drop = control * friction * Time.deltaTime;
      var newSpeed = speed - drop;
      if (newSpeed < 0) {
        newSpeed = 0;
      }
      newSpeed /= speed;
      velocity *= newSpeed;
    }
  }

  private void Accelerate(float accel)
  {
    var vel = new Vector2(velocity.x, velocity.z);
    var wishDir = new Vector2(wishMove.x, wishMove.z).normalized;
    var currentSpeed = Vector2.Dot(vel, wishDir);
    var addSpeed = moveSpeed - currentSpeed;
    addSpeed = Mathf.Max(Mathf.Min(addSpeed, accel * Time.deltaTime * moveSpeed), 0);
    velocity.x += addSpeed * wishDir.x;
    velocity.z += addSpeed * wishDir.y;
  }
}
