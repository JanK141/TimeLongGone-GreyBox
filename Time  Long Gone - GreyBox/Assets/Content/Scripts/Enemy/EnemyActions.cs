using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyAI))]
public abstract class EnemyActions : MonoBehaviour
{
    public abstract List<Action> Actions { get; }
    public abstract bool IsPerforming { get; set; }

}
