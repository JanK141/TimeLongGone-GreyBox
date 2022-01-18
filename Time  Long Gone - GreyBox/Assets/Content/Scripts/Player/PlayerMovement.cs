using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Basic movement")] [SerializeField]
    private float speed = 10;

    [SerializeField] private float gravity = -30;
    [SerializeField] private float jumpHeight = 10;
    [Header("Dashing")] [SerializeField] private float dashCD = 1f;

    [SerializeField] [Tooltip("How long dash animation is")]
    private float dashTime = 0.25f;

    [SerializeField] [Tooltip("What product of speed is applied every frame of a dash")]
    private float dashMoveMultiplier = 2f;

    [Header("Dash i-frames")]
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("In what percent of dash animation i-frames start")]
    float iframesStart = 0;

    [SerializeField] [Range(0, 1)] [Tooltip("In what percent of dash animation i-frames end")]
    private float iframesEnd = 0.3f;

    [Space] [SerializeField] private LayerMask groundMask;

    public float IFramesStart
    {
        get => iframesStart;
    }

    public float IFramesEnd
    {
        get => iframesEnd;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float DashTime
    {
        get => dashTime;
    }

    public bool CanDash
    {
        get => canDash;
        set => canDash = value;
    }

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public bool IsInvincible
    {
        get => isInvincible;
        set => isInvincible = value;
    }

    private Vector3 velocity;
    private Vector3 move;
    private bool isGrounded;
    private bool canDash = true;
    private bool canMove = true;
    private bool isInvincible = false;

    private CharacterController controller;

    void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x,
                transform.position.y - controller.height / 2,
                transform.position.z),
            0.4f,
            groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = new Vector3(x * Mathf.Sqrt(1 - z * z * 0.5f), 0, z * Mathf.Sqrt(1 - x * x * 0.5f));

        if (canMove && move.magnitude > 0.05)
        {
            controller.Move(move * speed * Time.unscaledDeltaTime);
            transform.LookAt(transform.position + move);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.unscaledDeltaTime;
        controller.Move(velocity * Time.unscaledDeltaTime);

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
        while (time < dashTime)
        {
            if (!isInvincible && time >= iframesStart * dashTime) isInvincible = true;
            if (isInvincible && time >= iframesEnd * dashTime) isInvincible = false;
            time += Time.unscaledDeltaTime;
            controller.Move(motion * speed * dashMoveMultiplier * Time.unscaledDeltaTime);
            yield return new WaitForEndOfFrame();
        }

        canMove = true;
    }

    void ResetDashCD() => canDash = true;

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color32(10, 200, 100, 200);
        if (GetComponent<DrawGizmos>().drawGizmos)
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, speed * dashMoveMultiplier * dashTime);
    }
}