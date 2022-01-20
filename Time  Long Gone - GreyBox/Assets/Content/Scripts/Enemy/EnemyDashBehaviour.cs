using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyDashBehaviour : StateMachineBehaviour
{
    [SerializeField] private AnimationCurve DashSpeedCurve;
    [SerializeField] private Status AttackStatus = Status.Regular;
    [SerializeField] private float PushForce;

    private Vector3 playerPos;
    private Vector3 enemyPos;
    private float duration;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyStatus.Instance.status = AttackStatus;
        EnemyStatus.Instance.AttackForce = PushForce;
        enemyPos = animator.transform.position;
        playerPos = new Vector3(PlayerMovement.Instance.transform.position.x, enemyPos.y, PlayerMovement.Instance.transform.position.z);
        animator.transform.DOLookAt
        (new Vector3(playerPos.x, animator.transform.position.y, playerPos.z), 0.25f);
        duration = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration += Time.deltaTime;
        animator.transform.position = Vector3.Lerp(enemyPos, playerPos, DashSpeedCurve.Evaluate(duration / stateInfo.length));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyStatus.Instance.status = Status.Neutral;
    }
}
