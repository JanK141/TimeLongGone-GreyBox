using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private bool beAgresive;
    private float distance;
    private Vector3 playerPosition;
    private GameObject player;

    private EnemyActions actions;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        actions = GetComponent<EnemyActions>();
    }
    

    void Update()
    {
        //CountDistance();
        //Behavoir();
        if (!actions.IsPerforming)
        {
            int rand = Random.Range(0, actions.Actions.Count);
            actions.Actions[rand].Invoke();
        }
    }

    private void Behavoir()
    {
        if (distance <= 10)
            beAgresive = true;
        else if (distance <= 1)
            GetComponent<EnemyAttack>().Attack();
        else if (distance > 10)
            beAgresive = false;

        if (beAgresive)
            GetComponent<EnemyWalk>().WalkTo(playerPosition);
    }

    private void CountDistance()
    {
        playerPosition = player.transform.position;
        distance = Vector3.Distance(playerPosition, transform.position);
        //  print("distance " + distance);
    }
}