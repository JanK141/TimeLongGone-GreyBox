using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyAttackBehaviour : StateMachineBehaviour
{
    [SerializeField] [Tooltip("Trigger + % chance for it to chain (order matters)")] private List<ListWrapper> TriggersToChain;
    [SerializeField] [Tooltip("Should enemy rotate towards player at animation start")] private bool LookAtPlayerAtStart;
    [SerializeField] [Tooltip("Should enemy be rotated towards player for whole animation")] private bool LookAtPlayer;
    [SerializeField] [Tooltip("Walk towards player during animation")] private bool FallowPlayer;
    [SerializeField] private Status AttackStatus = Status.Regular;
    [SerializeField] private float PushForce;

    private Transform player;
    private EnemyWalk walk;
    private int stage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stage = EnemyHealth.Instance.CurrStage;
        EnemyStatus.Instance.status = AttackStatus;
        EnemyStatus.Instance.AttackForce = PushForce;
        player = PlayerMovement.Instance.transform;
        walk = animator.GetComponent<EnemyWalk>();
        if(LookAtPlayerAtStart)
            if (LookAtPlayer)
                animator.transform.LookAt(new Vector3(player.position.x, animator.transform.position.y,
                    player.position.z));
            else
                animator.transform.DOLookAt(
                    new Vector3(player.position.x, animator.transform.position.y, player.position.z), 0.25f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(LookAtPlayer) animator.transform.LookAt(new Vector3(player.position.x, animator.transform.position.y,
            player.position.z));
        if(FallowPlayer) walk.WalkTo(player.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TriggersToChain.Count >= stage)
        {
            if (TriggersToChain[stage - 1].Pairs.Count > 0)
            {
                for (int i = 0; i < TriggersToChain[stage - 1].Pairs.Count; i++)
                {
                    if (Random.value <= TriggersToChain[stage - 1].Pairs[i].Chance)
                    {
                        animator.SetTrigger(TriggersToChain[stage - 1].Pairs[i].Trigger);
                        return;
                    }
                }
            }
        }

        EnemyStatus.Instance.status = Status.Neutral;
    }


    [System.Serializable]
    public class TriggerChancePair
    {
        public string Trigger;
        [Range(0,1)] public float Chance;
    }
    [System.Serializable]
    public class ListWrapper
    {
        public List<TriggerChancePair> Pairs;
    }
}
