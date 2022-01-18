using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkBehaviour : StateMachineBehaviour
{
    [SerializeField] private List<string> DashTriggers;
    [SerializeField] [Range(0, 1)] private float DashChance;

    private EnemyWalk enemyWalk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyWalk = animator.GetComponent<EnemyWalk>();
        if (DashTriggers.Count > 0)
        {
            if (Random.value < DashChance)
            {
                animator.SetTrigger(DashTriggers[Random.Range(0, DashTriggers.Count)]);
                return;
            }
        }
        enemyWalk.WalkTo(PlayerMovement.Instance.transform.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyWalk.CurrDestination = PlayerMovement.Instance.transform.position;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
