using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private bool beAgresive;
    private float distance;
    private Vector3 playerPosition;
    private GameObject player;

    // Start is called before the first frame update
    void Start() => player = GameObject.FindGameObjectWithTag("Player");

    // Update is called once per frame
    void Update()
    {
        CountDistance();
        Behavoir();
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