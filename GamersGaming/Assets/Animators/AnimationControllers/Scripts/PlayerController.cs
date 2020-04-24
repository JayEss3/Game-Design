using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator thisAnim;
    private Rigidbody rigid;
    public float groundDistance = 0.3f;
    public LayerMask whatIsGround;

    //Init
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    //Update once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        thisAnim.SetFloat("Speed", v);
        thisAnim.SetFloat("Turning Speed", h);
        if (Physics.Raycast (transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, whatIsGround))
        {
            thisAnim.SetBool("grounded", true);
            thisAnim.applyRootMotion = true;
        }
        else
        {
            thisAnim.SetBool("grounded", false);
        }
    }
}
