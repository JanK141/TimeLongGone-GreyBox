using System;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts.Time
{
    public class TimeBody : MonoBehaviour
    {
        bool isRewinding;

        List<Vector3> positions;

        private void Start()
        {
            positions = new List<Vector3>();
        }

        private void Update()
        {
            if (Input.GetKey("r"))
                if (isRewinding)
                    StopRewind();
                else
                    StartRewind();
        }

        private void FixedUpdate()
        {
            if (isRewinding)
                rewind();
            else
                Record();
        }

        void Record()
            =>
                positions.Insert(0, transform.position);

        public void rewind()
        {
            if (isRewinding)
                StartRewind();
            else
                StopRewind();
        }


        public void StartRewind()
        {
            isRewinding = true;
        }

        public void StopRewind()
        {
            isRewinding = false;
        }
    }
}