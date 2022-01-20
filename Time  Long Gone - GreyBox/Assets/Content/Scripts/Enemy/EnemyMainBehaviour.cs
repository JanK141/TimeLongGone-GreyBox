using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainBehaviour : StateMachineBehaviour
{
    [SerializeField] [Tooltip("Each List is separate stage. Always keep walk trigger as first")] private List<ListWrapper> TriggersToUse;
    [SerializeField] [Min(0)] private float MinWaitTime;
    [SerializeField] [Min(0)] private float MaxWaitTime;

    private float DistanceToTriggerWalk;
    private float timeToTrigger;
    private bool needToTrigger = false;
    private int stage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stage = EnemyHealth.Instance.CurrStage;
        DistanceToTriggerWalk = AIvariables.Instance.DistanceToTriggerWalk;
        if(Vector3.Distance(animator.transform.position, PlayerMovement.Instance.transform.position) >= DistanceToTriggerWalk)
            animator.SetTrigger(TriggersToUse[stage-1].Triggers[0]);
        else
        {
            timeToTrigger = Time.time + (Random.Range(MinWaitTime, MaxWaitTime));
            needToTrigger = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (needToTrigger && Time.time >= timeToTrigger)
        {
            animator.SetTrigger(TriggersToUse[stage-1].Triggers[Random.Range(1, TriggersToUse[stage-1].Triggers.Count)]);
            needToTrigger = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    [System.Serializable]
    public class ListWrapper
    {
        public List<string> Triggers;
    }

}
