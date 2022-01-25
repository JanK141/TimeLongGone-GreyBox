using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : MonoBehaviour
{
    private PlayerDefense Instance;

    [SerializeField] private float InvincibleTimeAfterCol;
    [SerializeField] private float BlockCooldown;
    [SerializeField] private float ParryWindow;
    [SerializeField] [Tooltip("Better keep it low")] private float PushFactor = 0.05f;

    private bool isColliding = false;
    private bool isBlocking = false;
    private bool canBlock = true;
    private float blockTime;

    private PlayerMovement pm;
    private EnemyStatus status;


    //TODO later make sure that player can't be blocking while dashing or attacking, or even while jumping (can block in air but not while performing take-off)
    void Awake() => Instance = this;
    void Start()
    {
        pm = PlayerMovement.Instance;
        status = EnemyStatus.Instance;
    }

    void Update()
    {
        if (canBlock && Input.GetButtonDown("Block"))
        {
            isBlocking = true;
            blockTime = Time.time;
        }

        if (isBlocking && Input.GetButtonUp("Block"))
        {
            isBlocking = false;
            canBlock = false;
            Invoke(nameof(ResetBlock), BlockCooldown);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("EnemyWeapon")) return;
        if (isColliding) return;

        HandleHit();
        isColliding = true;
        Invoke(nameof(ResetCollision), InvincibleTimeAfterCol);
    }

    void HandleHit()
    {
        Vector3 pushDir = ((transform.position - status.transform.position).normalized + Vector3.up) * status.AttackForce * PushFactor;

        switch (status.status)
        {
            case Status.Regular:
                if(pm.IsInvincible) return;
                if (isBlocking){
                    if (Time.time - blockTime <= ParryWindow){
                        StartCoroutine(Push(pushDir / 2));
                        status.Parried();
                        //TODO stuff
                        return;

                    }
                    StartCoroutine(Push(pushDir / 1.5f));
                    //TODO stuff
                    return;
                }
                break;
            case Status.Unblockable:
                if(pm.IsInvincible) return;
                if (isBlocking && Time.time - blockTime <= ParryWindow){
                    StartCoroutine(Push(pushDir / 2));
                    status.Parried();
                    //TODO stuff
                    return;
                }
                break;
            case Status.Undodgeable:
                if (isBlocking && Time.time - blockTime <= ParryWindow){
                    StartCoroutine(Push(pushDir / 2));
                    status.Parried();
                    //TODO stuff
                    return;
                }
                break;
            case Status.Unstoppable:
                break;
            default:
                return;
        }

        StartCoroutine(Push(pushDir));
        print("DEATH!"); //TODO stuff
        TimeManipulation.Instance.IsPlayerDead = true;
    }

    IEnumerator Push(Vector3 force)
    {
        float grav = 0;
        float i = 0.05f;
        while (force.magnitude > i)
        {
            pm.Velocity = new Vector3(force.x / i, (force.y / i)+grav, force.z / i);
            grav += pm.Gravity * 0.5f * Time.deltaTime;
            i += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void ResetCollision() => isColliding = false;

    void ResetBlock() => canBlock = true;
}
