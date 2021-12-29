using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float attackRadius = 1;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 0.2f;
    [SerializeField] private float minHoldTime = 0.8f;
    [SerializeField] private float maxHoldTime = 2f;
    [SerializeField] private Collider ChargedAttackTrigger;

    private bool canAttack = true;
    private float attackPressedTime;
    private float moveSpeed;
    private bool isKeyDown = false;

    private PlayerMovement pm;
    private ChargedAttackTrigger cat;
    private CharacterController controller;
    private Transform enemy;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        cat = ChargedAttackTrigger.GetComponent<ChargedAttackTrigger>();
        controller = GetComponent<CharacterController>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    void Start()
    {
        moveSpeed = pm.Speed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //TODO while holding attack button draw some indicator showing distance and direction of dash-attack
            if (canAttack)
            {
                isKeyDown = true;
                attackPressedTime = Time.time;
                pm.Speed = moveSpeed * 0.2f;
            }
            else
            {
                //TODO some vfx or sfx (or both) indicating that action can't be performed
            }
        }
        if (Input.GetButtonUp("Fire2") && canAttack && isKeyDown)
        {
            isKeyDown = false;
            pm.Speed = moveSpeed;
            float holdTime = Time.time - attackPressedTime;
            if (holdTime < minHoldTime)
            {
                canAttack = false;
                Attack();
                Invoke(nameof(ResetAttack), attackCooldown);
            }
            else
            {
                canAttack = false;
                StartCoroutine(DashAttack(Mathf.Clamp(holdTime, minHoldTime*2, maxHoldTime)));
                Invoke(nameof(ResetAttack), attackCooldown + attackCooldown* Mathf.Clamp(holdTime, minHoldTime * 2, maxHoldTime));
            }
        }
    }

    void Attack()
    {
        if(Physics.CheckSphere(transform.position + (transform.forward * attackDistance), attackRadius, enemyLayerMask))
        {
            print("DAMAGE! " + damage);
        }
    }

    IEnumerator DashAttack(float strength)
    {
        cat.gameObject.SetActive(true);
        cat.damage = damage + damage * strength;
        Physics.IgnoreCollision(controller, enemy.GetComponent<Collider>(), true);
        pm.CanDash = false; pm.CanMove = false;
        Vector3 motion = transform.forward;
        float time = 0f;
        while (time < pm.DashTime)
        {
            time += Time.deltaTime;
            controller.Move(motion * pm.Speed * strength * 2.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        pm.CanDash = true; pm.CanMove = true;
        Physics.IgnoreCollision(controller, enemy.GetComponent<Collider>(), false);
        cat.gameObject.SetActive(false);
    }

    void ResetAttack() => canAttack = true;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(200, 20, 20, 100);
        if(GetComponent<DrawGizmos>().drawGizmos)Gizmos.DrawSphere(transform.position + (transform.forward*attackDistance), attackRadius);
    }
}
