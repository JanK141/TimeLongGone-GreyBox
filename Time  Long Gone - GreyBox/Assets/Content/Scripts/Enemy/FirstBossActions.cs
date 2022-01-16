using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyAI))]
public class FirstBossActions : EnemyActions
{
    public override List<Action> Actions { get=>actions; }
    public override bool IsPerforming { get=>isPerforming; set=>isPerforming=value; }

    private List<Action> actions;
    private bool isPerforming = false;

    private Animator animator;
    private EnemyWalk walk;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        walk = GetComponent<EnemyWalk>();

        actions = new List<Action>()
        {
            async () =>
            {
                //Here we can again check distance to player and randomly walk to him or use other action with some dash/leap attack
                walk.WalkTo(player.position);
                while (walk.IsWalking)
                    await Task.Delay((int) (Time.deltaTime * 1000));
            },
            async () =>
            {
                isPerforming = true;
                animator.SetTrigger("Attack2");
                while (!animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("EnemyAttack02")) //Wait for animation to start
                    await Task.Delay((int) (Time.deltaTime * 1000));
                while (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("EnemyAttack02")) //Wait for animation to end
                    await Task.Delay((int) (Time.deltaTime * 1000));
                if (Random.value >= 0.5f) //50% chance to chain different attack
                    actions[2].Invoke();
                else
                    isPerforming = false; //Otherwise end
            },
            async () =>
            {
                isPerforming = true;
                animator.SetTrigger("Attack1");
                while (!animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("EnemyAttack01"))
                    await Task.Delay((int) (Time.deltaTime * 1000));
                while (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("EnemyAttack01"))
                    await Task.Delay((int) (Time.deltaTime * 1000));
                isPerforming = false;
            }
        };
    }
}
