using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField] private Text timeCounter;

    private TimeSpan timePlaying;
    private float elapsedTime;
    private bool timerGoing = false;

    public bool TimerGoing{get=>timerGoing; set{timerGoing=value; BeginTimer();}}

    void Awake() => Instance = this;
    void Start()
    {
        timeCounter.text = "00:00:0";
        elapsedTime = 0f;
        TimerGoing = true;
    }
    
    void BeginTimer()
    {
        if(!timerGoing) return;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            var timePlayingStr = timePlaying.ToString("mm':'ss':'f");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }
}
