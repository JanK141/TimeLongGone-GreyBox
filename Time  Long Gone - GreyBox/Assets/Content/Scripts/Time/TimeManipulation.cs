using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TimeManipulation : MonoBehaviour
{
    public static TimeManipulation Instance;

    [SerializeField] [Range(0,1)] [Tooltip("How low time scale can get during slow mo")] private float MinSlowMo;
    [SerializeField] [Tooltip("How long it gets to bring time scale to it's minimum value")] private float SlowMoTime;
    [SerializeField] [Tooltip("How much faster time should slow down when player is killed")] private float DeathSlowMoMulti=3;
    [SerializeField] private float MaxRewindTime;
    [SerializeField] private CinemachineVirtualCamera cam;

    private bool isRewinding = false;
    private bool isPlayerDead = false;

    public bool IsPlayerDead{get=>isPlayerDead; set{isPlayerDead=value; DeadOrAlive(value);}}
    public float RewindTime{get=>MaxRewindTime;}
    public bool IsRewinding{get=>isRewinding;}

    void Awake() => Instance = this;
    void Start()
    {

    }

    void Update()
    {
        if (!isPlayerDead)
        {
            if (Input.GetButtonDown("Power")) StartCoroutine(StartSlowMo());
            if (Input.GetButtonUp("Power")) StartCoroutine(StopSlowMo());
        }
        else
        {
            if (Input.GetButtonDown("Power"))
            {
                PlayerMovement.Instance.GetComponent<CharacterController>().enabled = false;
                EnemyStatus.Instance.GetComponent<Animator>().enabled = false;
                isRewinding =true; 
                print("Start Rewind");
            }
            if (Input.GetButtonUp("Power"))
            {
                PlayerMovement.Instance.GetComponent<CharacterController>().enabled = true;
                EnemyStatus.Instance.GetComponent<Animator>().enabled = true;
                print("Stop Rewind");
                isRewinding = false;
                IsPlayerDead = false;
            }
        }
    }

    void DeadOrAlive(bool dead)
    {
        if (dead) StartCoroutine(PlayerDead());
        else
        {
            Time.timeScale = MinSlowMo;
            StartCoroutine(StopSlowMo());
        }
    }

    IEnumerator StartSlowMo()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = ((1 - Time.timeScale) / (1 - MinSlowMo)) * SlowMoTime;
        while (Input.GetButton("Power") && Time.timeScale>MinSlowMo)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(88f, 60f, time / SlowMoTime);
            Time.timeScale = Mathf.Lerp(1f, MinSlowMo, time / SlowMoTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        if (Input.GetButton("Power"))
        {
            Time.timeScale = MinSlowMo;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
    //TODO change loop conditions in corutines for safety
    IEnumerator StopSlowMo()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = ((Time.timeScale-MinSlowMo) / (1 - MinSlowMo)) * SlowMoTime;
        while (!Input.GetButton("Power") && Time.timeScale < 1f)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(60f, 88f, time / SlowMoTime);
            Time.timeScale = Mathf.Lerp(MinSlowMo, 1f, time / SlowMoTime);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
        if (!Input.GetButton("Power"))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    IEnumerator PlayerDead()
    {
        var composer = cam.GetCinemachineComponent<CinemachineGroupComposer>();
        float time = (1 - Time.timeScale) * (SlowMoTime/DeathSlowMoMulti);
        while (Time.timeScale>0.01f)
        {
            time += Time.unscaledDeltaTime;
            composer.m_MinimumFOV = Mathf.Lerp(88f, 40f, time / (SlowMoTime / DeathSlowMoMulti));
            Time.timeScale = Mathf.Lerp(1f, 0.01f, time / (SlowMoTime/DeathSlowMoMulti));
            //Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }
}
