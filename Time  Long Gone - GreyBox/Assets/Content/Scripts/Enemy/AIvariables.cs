using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is purely to unpack some Behaviour classes variables outside the state machine to make them easier to modify all at once
//and to be able to draw them in edit mode for more clear view on how they will affects gameplay
public class AIvariables : MonoBehaviour
{
    public static AIvariables Instance;

    [SerializeField] public float DistanceToTriggerWalk;

    void Awake() => Instance = this;

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color32(10, 200, 200, 200);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToTriggerWalk);
    }
}
