using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyStatus : MonoBehaviour
{
    public static EnemyStatus Instance;
    private Status _status;

    public Status status { get=>_status; set=>_status=value;  }

    void Awake() => Instance = this;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStatus()
    {
        //TODO Here is to handle any indicator display or smth that accords to a current state
    }

}

public enum Status
{
    Neutral, //Neutral status; Boss is just minding his own businesses
    Regular, //Regular attack; Can be blocked, dodged, parried, avoided 
    Unblockable, //Attack you can dodge or parry, but not block
    Unparryable, //Attack you can't block or parry, only dodge or avoid
    Undogeable, //This attack don't give a shit about your freaking i-frames
    Unstopable, //The only thing you can do about this attack is to get as far away as you can 
    Stunable, //Status in which enemy can be stunned
    Stunned, //He feels a bit dizzy or smth and can't do shit about us attacking him
    Invincible //Can't be harmed as he is performing some action like starting new stage 
}
