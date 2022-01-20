using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyStatus : MonoBehaviour
{
    public static EnemyStatus Instance;

    [SerializeField] private List<Status> ParryAffected;
    [SerializeField] [Range(0,1)] private float ParriedChance;

    private Status _status;
    private float attackForce;
    private Animator anim;

    public Status status { get=>_status; set=>_status=value;  }
    public float AttackForce {get=>attackForce; set=>attackForce=value;}

    void Awake() => Instance = this;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStatus()
    {
        //TODO Here is to handle any indicator display or smth that accords to a current state
    }

    public void Parried()
    {
        foreach (Status s in ParryAffected)
        {
            if (s == _status && Random.value < ParriedChance)
            {
                anim.SetTrigger("Parried");
                return;
            }
        }
    }

}

public enum Status
{
    Neutral, //Neutral status; Boss is just minding his own businesses
    Regular, //Regular attack; Can be blocked, dodged, parried, avoided 
    Unblockable, //Attack you can dodge or parry, but not block
    Undodgeable, //This attack don't give a shit about your freaking i-frames though you can parry it
    Unstoppable, //The only thing you can do about this attack is to get as far away as you can 
    Stunnable, //Status in which enemy can be stunned
    Stunned, //He feels a bit dizzy or smth and can't do shit about us attacking him
    Invincible //Can't be harmed as he is performing some action like starting new stage 
}
