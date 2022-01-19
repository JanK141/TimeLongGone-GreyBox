using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public Text timeCounter;

//  slow-MO variable
    private float SlowDownfactor = 0.05f;
    private float SlowDownLenght = 2f;

    // Timer variable
    private TimeSpan timePlaying;
    private float elapsedTime;

    private bool timerGoing;
    private bool isPaused;

    private void Awake()
        => instance = this;

    private void Start()
    {
        timeCounter.text = "00:00:0";
        BeginTimer();
        isPaused = false;
    }

    public void Update()
    {
        Time.timeScale += 1f / SlowDownLenght * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        
        InputKeyController();
    }

    private void InputKeyController()
    {
        if (Input.GetKey("p")) Pause();

        if (Input.GetKey("t")) DoSlowMo();
    }


    private void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void DoSlowMo()
    {
        Time.timeScale = SlowDownfactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void BeginTimer()
    {
        // begin time at 00:00:0
        timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer() => timerGoing = false;

    private IEnumerator UpdateTimer()
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