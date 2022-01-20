using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMidBehaviour : StateMachineBehaviour
{
    [SerializeField] private Status DefaultStatus;
    [SerializeField] private Status AlternativeStatus;
    [SerializeField] [Range(0,1)] private float AlternativeChance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Random.value < AlternativeChance) EnemyStatus.Instance.status = AlternativeStatus;
        else EnemyStatus.Instance.status = DefaultStatus;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyStatus.Instance.status = Status.Neutral;
    }
}
