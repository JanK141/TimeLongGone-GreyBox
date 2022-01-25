using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour
{
    private LinkedList<Vector3> positions;
    private LinkedList<Quaternion> rotations;

    private int maxNodes;
    private bool rewind = true;

    private TimeManipulation time;
    void Start()
    {
        positions = new LinkedList<Vector3>();
        rotations = new LinkedList<Quaternion>();
        time = TimeManipulation.Instance;
        maxNodes =(int) (time.RewindTime / Time.fixedDeltaTime);
    }


    void FixedUpdate()
    {
        if (!time.IsRewinding)
        {
            positions.AddLast(transform.position);
            rotations.AddLast(transform.rotation);
            if (positions.Count >= maxNodes)
            {
                positions.RemoveFirst();
                rotations.RemoveFirst();
            }
        }
        
    }

    void Update()
    {
        if (time.IsRewinding && rewind)
        {
            rewind = false;
            StartCoroutine(GoBack());
        }
    }

    IEnumerator GoBack()
    {
        while (time.IsRewinding)
        {
            if (positions.Count > 0)
            {
                var pos = transform.position;
                var rot = transform.rotation;
                for (int i = 1; i <= 30; i++)
                {
                    transform.position = Vector3.Lerp(pos, positions.Last.Value, (float)i/30);
                    transform.rotation = Quaternion.Lerp(rot, rotations.Last.Value, (float)i/30);
                    yield return null;
                }

                positions.RemoveLast();
                rotations.RemoveLast();
            }
        }
        rewind = true;
    }
}
