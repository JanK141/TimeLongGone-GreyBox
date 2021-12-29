using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float gravity = -30;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float dashCD = 1f;
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private float dashMoveMultiplier = 2f;

    public float Speed{get=>speed; set=>speed=value;}
    public float DashTime{get=>dashTime;} 
    public bool CanDash{get=>canDash; set=>canDash=value;}
    public bool CanMove{get=>canMove; set=>canMove=value;}

    private Vector3 velocity;
    private Vector3 move;
    private bool isGrounded;
    private bool canDash = true;
    private bool canMove = true;

    private CharacterController controller;

    void Awake() => controller = GetComponent<CharacterController>();
    void Update()
    {
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y-controller.height/2, transform.position.z), 0.4f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = new Vector3(x*Mathf.Sqrt(1-z*z*0.5f), 0, z*Mathf.Sqrt(1-x*x*0.5f));

        if (canMove && move.magnitude>0.05)
        {
            controller.Move(move * speed * Time.deltaTime);
            transform.LookAt(transform.position+move);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && canDash)
        {
            canDash = false;
            StartCoroutine(nameof(Dash));
            Invoke(nameof(ResetDashCD), dashCD);
        }
    }

    IEnumerator Dash()
    {
        canMove = false;
        Vector3 motion = move.normalized;
        float time = 0f;
        while (time<dashTime)
        {
            time += Time.deltaTime;
            controller.Move(motion * speed * dashMoveMultiplier * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        canMove = true;
    }

    void ResetDashCD() => canDash = true;

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color32(10, 200, 100, 200);
        if (GetComponent<DrawGizmos>().drawGizmos) UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, speed * dashMoveMultiplier * dashTime);
    }
}
